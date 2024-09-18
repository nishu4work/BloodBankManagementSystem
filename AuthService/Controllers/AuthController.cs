using AuthService.DAL;
using AuthService.DTO;
using AuthService.IRepository;
using AuthService.Models;
using AuthService.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

using System.Text;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public AuthController(IConfiguration config, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _config = config;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegister userRegister)
        {
            try
            {
                // Check if the user already exists
                var existingUser = await _userRepository.GetUserByEmail(userRegister.Email);
                if (existingUser != null)
                {
                    return BadRequest("User already exists !!");
                }

                // Fetch role based on the role name provided by the user
                var role = await _roleRepository.GetRoleByName(userRegister.RoleName);
                if (role == null)
                {
                    return BadRequest("Invalid role !!");
                }

                //Create salt and password hash for secure storage.
                string passwordSalt = GenerateSalt();
                string passwordHash = GeneratePasswordHash(userRegister.Password,passwordSalt);
                
                // Create a new user
                var user = new User
                {
                    Email = userRegister.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    RoleId = role.RoleId
                };

                await _userRepository.AddUser(user);
                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(userLogin.Email);

                if (user != null)
                {
                    string passwordHash = GeneratePasswordHash(userLogin.Password, user.PasswordSalt);
                    bool isPasswordCorrect = string.Equals(passwordHash, user.PasswordHash);
                    if (isPasswordCorrect)
                    {
                        var token = GenerateJwtToken(user);
                        return Ok(new { token, user });
                    }
                    else
                        return BadRequest("Incorrect Password. Please check and try again.");
                }

                return BadRequest("This Email is not registered. Please do registration or check Email.");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userRepository.GetAllUsers();
                if (users.Any())
                {
                    var result = users.Select(u => new
                    {
                        u.UserId,
                        u.Email,
                        Role = u.Role.RoleName
                    });
                    return Ok(result);
                }
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateUser/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserUpdate userUpdate)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Update user properties
            user.Email = userUpdate.Email ?? user.Email;

            if (userUpdate.Password != null)
            {
                //Create salt and password hash for secure storage.
                string passwordSalt = GenerateSalt();
                string passwordHash = GeneratePasswordHash(userUpdate.Password, passwordSalt);

                user.PasswordSalt = passwordSalt;
                user.PasswordHash = passwordHash;

            }

            // Update role if necessary
            if (userUpdate.RoleId != null)
            {
                var role = await _roleRepository.GetRoleById((int)userUpdate.RoleId);
                if (role == null)
                {
                    return BadRequest("Invalid role.");
                }
                user.RoleId = (int)userUpdate.RoleId;
            }

            await _userRepository.UpdateUser(user);
            return Ok("User updated successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            await _userRepository.DeleteUser(userId);
            return Ok("User deleted successfully.");
        }
        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.RoleName)
        };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                                             _config["Jwt:Audience"],
                                             claims,
                                             expires: DateTime.Now.AddMinutes(120),
                                             signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateSalt()
        {
            //Genrating a 128-bit salt 
            string salt = Encoding.UTF8.GetString(RandomNumberGenerator.GetBytes(16));
            return salt;
        }

        private string GeneratePasswordHash(string password, string salt)
        {
            string combinedPassword = password + salt;
            using (var sha = SHA256.Create())
            {
                // Compute hash of combined password
                byte[] bytes = Encoding.UTF8.GetBytes(combinedPassword);
                byte[] hash = sha.ComputeHash(bytes);

                //Convert the byte array into hexadecimal string
                string hashedPassword = Convert.ToHexString(hash);
                return hashedPassword;

            }
        }
    }

}
