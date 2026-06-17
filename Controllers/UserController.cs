using Microsoft.AspNetCore.Mvc;
using VenderTest.DTOs;
using VenderTest.Service;

namespace VenderTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IForgetService _forgetService;

        public UserController(IUserService userService, IForgetService forgetService)
        {
            _userService = userService;
            _forgetService = forgetService;
        }

        // ========================
        // LOGIN
        // ========================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Invalid request");

                var result = await _userService.LoginAsync(
                    request.Email,
                    request.password   
                );

                if (result == null)
                {
                    return BadRequest(new
                    {
                        Status = 0,
                        Message = "Invalid email or password"
                    });
                }

                return Ok(new
                {
                    Status = 1,
                    Message = "Login successful",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = 0,
                    Message = "Server error",
                    Error = ex.Message
                });
            }
        }

        // ========================
        // REGISTER
        // ========================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Invalid request");

                var result = await _userService.RegisterUserAsync(
                    request.Email,
                    request.password,   // ✅ FIXED
                    request.VenderCode
                );

                if (result == null || result.UserId == 0)
                {
                    return BadRequest(new
                    {
                        Status = 0,
                        Message = result?.Message ?? "Registration failed"
                    });
                }

                return Ok(new
                {
                    Status = 1,
                    Message = result.Message,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = 0,
                    Message = "Server error",
                    Error = ex.Message
                });
            }
        }

        // ========================
        // SEND OTP
        // ========================
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] ForgetDto request)
        {
            if (request == null ||
                string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.VenderCode))
            {
                return BadRequest(new ApiResponseDto
                {
                    Status = 0,
                    Message = "Email & VendorCode required"
                });
            }

            var result = await _forgetService.SendOtpAsync(request.Email, request.VenderCode);
            return Ok(result);
        }

        // ========================
        // VERIFY OTP
        // ========================
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] ForgetDto request)
        {
            if (request == null ||
                string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.OtpCode))
            {
                return BadRequest(new ApiResponseDto
                {
                    Status = 0,
                    Message = "Email & OTP required"
                });
            }

            var result = await _forgetService.VerifyOtpAsync(request.Email, request.OtpCode);
            return Ok(result);
        }

        // ========================
        // RESET PASSWORD
        // ========================
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ForgetDto request)
        {
            if (request == null ||
                string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new ApiResponseDto
                {
                    Status = 0,
                    Message = "Email & Password required"
                });
            }

            var result = await _forgetService.ResetPasswordAsync(request.Email, request.Password);
            return Ok(result);
        }
    }
}