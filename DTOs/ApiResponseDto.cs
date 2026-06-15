
namespace VenderTest.DTOs
{
    public class ApiResponseDto
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public string? IsActive { get; set; }

        public string? OtpCode { get; set; }
        public List<object>? Data { get; internal set; }
    }
}
