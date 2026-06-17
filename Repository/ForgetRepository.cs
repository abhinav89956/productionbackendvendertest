using Npgsql;
using VenderTest.CommonService;
using VenderTest.DTOs;

namespace VenderTest.Repository
{
    public class ForgetRepository : IForgetRepository
    {
        private readonly IGenericRepository _repo;
        private readonly Common _commonService;

        public ForgetRepository(IGenericRepository repo, Common common)
        {
            _repo = repo;
            _commonService = common;
        }

        public async Task<ApiResponseDto> SendOtpAsync(string email, string venderCode)
        {
            try
            {
                var spResult = await _repo.QueryFirstOrDefaultAsync<OtpResultDto>(
                    @"SELECT * FROM ""_vender"".sp_sendotp(
                        @Email,
                        @VenderCode)",
                    new
                    {
                        Email = email,
                        VenderCode = venderCode
                    });

                if (spResult == null || spResult.Status == 0)
                {
                    return new ApiResponseDto
                    {
                        Status = 0,
                        Message = spResult?.Message ?? "Invalid Email or Vendor Code"
                    };
                }

                var emailSent = await _commonService.SendOtpEmailAsync(
                    email,
                    spResult.OtpCode);

                if (!emailSent)
                {
                    return new ApiResponseDto
                    {
                        Status = 0,
                        Message = "OTP generated but email failed"
                    };
                }

                return new ApiResponseDto
                {
                    Status = 1,
                    Message = "OTP Sent Successfully"
                };
            }
            catch (NpgsqlException)
            {
                return new ApiResponseDto
                {
                    Status = 0,
                    Message = "Database error occurred"
                };
            }
            catch (Exception)
            {
                return new ApiResponseDto
                {
                    Status = 0,
                    Message = "Application error occurred"
                };
            }
        }

        public async Task<ApiResponseDto> VerifyOtpAsync(string email, string otp)
        {
            try
            {
                var spResult = await _repo.QueryFirstOrDefaultAsync<ApiResponseDto>(
                    @"SELECT * FROM ""_vender"".sp_verifyotp(
                        @Email,
                        @Otp)",
                    new
                    {
                        Email = email,
                        Otp = otp
                    });

                if (spResult == null || spResult.Status == 0)
                {
                    return new ApiResponseDto
                    {
                        Status = 0,
                        Message = spResult?.Message ?? "Invalid or Expired OTP"
                    };
                }

                return spResult;
            }
            catch (NpgsqlException)
            {
                return new ApiResponseDto
                {
                    Status = 0,
                    Message = "Database error occurred"
                };
            }
            catch (Exception)
            {
                return new ApiResponseDto
                {
                    Status = 0,
                    Message = "Application error occurred"
                };
            }
        }

        public async Task<ApiResponseDto> ResetPasswordAsync(string email, string password)
        {
            try
            {
                var spResult = await _repo.QueryFirstOrDefaultAsync<ApiResponseDto>(
                    @"SELECT * FROM ""_vender"".sp_resetpassword(
                        @Email,
                        @Password)",
                    new
                    {
                        Email = email,
                        Password = password
                    });

                if (spResult == null || spResult.Status == 0)
                {
                    return new ApiResponseDto
                    {
                        Status = 0,
                        Message = spResult?.Message ?? "Password reset failed"
                    };
                }

                return spResult;
            }
            catch (NpgsqlException)
            {
                return new ApiResponseDto
                {
                    Status = 0,
                    Message = "Database error occurred"
                };
            }
            catch (Exception)
            {
                return new ApiResponseDto
                {
                    Status = 0,
                    Message = "Application error occurred"
                };
            }
        }
    }
}