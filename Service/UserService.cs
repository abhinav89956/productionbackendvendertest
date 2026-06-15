using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using VenderTest.CommonService;
using VenderTest.DTOs;
using VenderTest.Models;
using VenderTest.Repository;
using YourProject.Models;

namespace VenderTest.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<UserDto?> LoginAsync(string email, string password)
        {
            try
            {

                var user = await _userRepository.LoginAsync(email, password);

                if (user == null || user.Status == 0)
                    return null;

                var token = GenerateJwtToken(user);


                return new UserDto
                {
                    UserId = user.UserId,
                    VenderCode = user.VenderCode,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    Token = token,
                    Status = user.Status,
                    Message = user.Message
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error while login", ex);
            }
        }

        private string GenerateJwtToken(dynamic user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("IsActive", user.IsActive.ToString()),
                new Claim("UserCode", user.UserCode ?? "")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
              expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<UserDto> RegisterUserAsync(string email, string password, string venderCode)
        {
           
            var result = await _userRepository.RegisterUserAsync(email, password, venderCode);

            
            if (result != null && result.Status == 1)
            {
                try
                {
                    
                    var vendor = new Vendor
                    {
                        VenderCode = venderCode,
                        Email = email
                    };

                   
                    var payload = new
                    {
                        userId = result.UserId,
                        email = email,
                        token = result.Token 
                    };

                 
                    string jsonPayload = JsonSerializer.Serialize(payload);
                    string encodedPayload = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonPayload));

               
                    string welcomeLink = $"http://localhost:4200/invite?data={encodedPayload}";

                    // 7️⃣ Send welcome email
                    var emailService = new CommanEmail(_configuration);
                    await emailService.SendWelcomeEmailAsync(vendor, welcomeLink);

                    Console.WriteLine($"Welcome email sent to {vendor.Email}");
                }
                catch (Exception ex)
                {
                
                    Console.WriteLine($"Failed to send welcome email: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("User registration not confirmed yet. Email not sent.");
            }

           
            return result;
        }
    }
}