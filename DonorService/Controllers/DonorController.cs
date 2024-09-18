using DonorService.DTO;
using DonorService.IRepository;
using DonorService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DonorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonorController : ControllerBase
    {
        private readonly IDonorRepository _donorRepository;

        public DonorController(IDonorRepository donorRepository)
        {
            _donorRepository = donorRepository;
        }

        [Authorize(Roles = "Donor")]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterDonor([FromBody] DonorRegister donorRegister)
        {
            var donor = new Donor
            {
                Name = donorRegister.Name,
                Age = donorRegister.Age,
                //BloodType = donorRegister.BloodType,
                Phone = donorRegister.Phone,
                //HealthStatus = donorRegister.HealthStatus
            };

            await _donorRepository.AddDonorAsync(donor);
            return Ok("Donor registered successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Donor>>> GetAllDonors()
        {
            var donors = await _donorRepository.GetAllDonorsAsync();
            return Ok(donors);
        }

        [Authorize(Roles = "Admin,Donor")]
        [HttpGet("GetDonor/{donarId}")]
        public async Task<ActionResult<Donor>> GetDonorById(int donarId)
        {
            
            var donor = await _donorRepository.GetDonorByIdAsync(donarId);
            if (donor == null)
            {
                return NotFound();
            }
            return Ok(donor);
        }

        [Authorize(Roles = "Admin,Donor")]
        [HttpPut("UpdateDonorDetails/{donarId}")]
        public async Task<IActionResult> UpdateDonor(int donarId, [FromBody] Donor donor)
        {
            if (donarId != donor.DonorId)
            {
                return BadRequest();
            }

            var existingDonor = await _donorRepository.GetDonorByIdAsync(donarId);
            if (existingDonor == null)
            {
                return NotFound();
            }

            await _donorRepository.UpdateDonorAsync(donor);
            return NoContent();
        }

        [Authorize(Roles = "Admin,Donor")]
        [HttpDelete("DeleteDonor/{id}")]
        public async Task<IActionResult> DeleteDonor(int id)
        {
            var donor = await _donorRepository.GetDonorByIdAsync(id);
            if (donor == null)
            {
                return NotFound();
            }

            await _donorRepository.DeleteDonorAsync(id);
            return NoContent();
        }
    }
}
