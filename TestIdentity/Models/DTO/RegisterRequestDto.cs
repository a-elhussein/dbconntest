namespace TestIdentity.Models.DTO
{
    public class RegisterRequestDto
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string[] Company { get; set; }
        public string[] Roles { get; set; }
    }
}
