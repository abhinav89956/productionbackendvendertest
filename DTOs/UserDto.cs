namespace VenderTest.DTOs
{
    public class UserDto
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public string? PaswdHash { get; set; }
        public string? password { get; set; }
        public string? VenderCode { get; set; }
        public int? UserId { get; set; }
        public string? Email { get; set; }

        public string? UserCode { get; set; }
        public bool? IsActive { get; set; }
        public string? Token { get; set; }
    }
}
