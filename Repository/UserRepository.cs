using BCrypt.Net;
using System.Security.Cryptography;
using System.Text;
using VenderTest.DTOs;
using VenderTest.Repository;

public class UserRepository : IUserRepository
{
    private readonly IGenericRepository _repo;

    public UserRepository(IGenericRepository repo)
    {
        _repo = repo;
    }
    private string HashPassword(string password)
    {
        using (SHA256 sha = SHA256.Create())
        {
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "");
        }
    }

    public async Task<UserDto> LoginAsync(string email, string password)
    {
        try
        {
            var user = await _repo.QueryFirstOrDefaultAsync<UserDto>(
            @"SELECT 
            ""UserId"",
            ""Email"",
            ""PaswdHash"",
            ""IsActive"",
            ""IsDeleted""
        FROM ""_vender"".""User""
        WHERE ""Email"" = @Email
        AND ""IsDeleted"" = FALSE",
            new { Email = email });

            if (user == null)
            {
                return new UserDto { Status = 0, Message = "Invalid email or password" };
            }

            // 🔥 SHA256 compare (CORRECT)
            if (user.PaswdHash != HashPassword(password))
            {
                return new UserDto { Status = 0, Message = "Invalid email or password" };
            }

            return new UserDto
            {
                Status = 1,
                Message = "Login successful",
                UserId = user.UserId,
                Email = user.Email
            };
        }
        catch (Exception ex)
        {
            return new UserDto { Status = 0, Message = ex.Message };
        }
    }

    // ================= REGISTER =================
    public async Task<UserDto> RegisterUserAsync(string email, string password, string venderCode)
    {
        try
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            var result = await _repo.QueryFirstOrDefaultAsync<UserDto>(
                @"SELECT * FROM ""_vender"".sp_registeruser(
                @Email,
                @Password,
                @VenderCode)",
                new
                {
                    Email = email,
                    Password = hash,
                    VenderCode = venderCode
                });

            return result ?? new UserDto
            {
                Status = 0,
                Message = "Registration failed"
            };
        }
        catch (Exception ex)
        {
            return new UserDto
            {
                Status = 0,
                Message = ex.Message
            };
        }
    
}
}