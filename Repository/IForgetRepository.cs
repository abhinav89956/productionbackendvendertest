using VenderTest.DTOs;

namespace VenderTest.Repository
{
    public interface IForgetRepository
    {
        Task<ApiResponseDto> SendOtpAsync(string email, string venderCode);

        Task<ApiResponseDto> VerifyOtpAsync(string email, string otp);

        Task<ApiResponseDto> ResetPasswordAsync(string email, string password);
    }
}

