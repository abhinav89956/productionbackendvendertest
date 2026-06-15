using VenderTest.DTOs;
using System.Threading.Tasks;

namespace VenderTest.Service
{
    public interface IForgetService
    {
        Task<ApiResponseDto> SendOtpAsync(string email, string venderCode);

        Task<ApiResponseDto> VerifyOtpAsync(string email, string otp);

        Task<ApiResponseDto> ResetPasswordAsync(string email, string password);
    }
}
