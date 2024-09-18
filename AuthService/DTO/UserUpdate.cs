namespace AuthService.DTO
{
    public class UserUpdate
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }
    }
}
