using System.Threading.Tasks;
using VenderTest.DTOs;
using VenderTest.Repository;

namespace VenderTest.Service
{
    public class ForgetService : IForgetService
    {
        private readonly IForgetRepository _forgetRepository;

        public ForgetService(IForgetRepository forgetRepository)
        {
            _forgetRepository = forgetRepository;
        }

        public async Task<ApiResponseDto> SendOtpAsync(string email, string venderCode)
        {
            var result = await _forgetRepository.SendOtpAsync(email, venderCode);
            return result;
        }

        public async Task<ApiResponseDto> VerifyOtpAsync(string email, string otp)
        {
            var result = await _forgetRepository.VerifyOtpAsync(email, otp);
            return result;
        }

        public async Task<ApiResponseDto> ResetPasswordAsync(string email, string password)
        {
            var result = await _forgetRepository.ResetPasswordAsync(email, password);
            return result;
        }
    }
}
