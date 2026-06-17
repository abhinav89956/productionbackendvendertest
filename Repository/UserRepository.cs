using BCrypt.Net;
using Npgsql;
using Org.BouncyCastle.Crypto.Generators;
using VenderTest.DTOs;
using VenderTest.Repository;

public class UserRepository : IUserRepository
{
    private readonly IGenericRepository _repo;

    public UserRepository(IGenericRepository repo)
    {
        _repo = repo;
    }

    public async Task<UserDto> LoginAsync(string email, string password)
    {
        try
        {
            // Step 1: Get user by email only
            var user = await _repo.QueryFirstOrDefaultAsync<UserDto>(
                @"SELECT * 
              FROM ""_vender"".""User"" 
              WHERE ""Email"" = @Email 
              AND ""IsDeleted"" = FALSE",
                new { Email = email });

            if (user == null)
            {
                return new UserDto
                {
                    Status = 0,
                    Message = "Invalid email or password"
                };
            }

            // Step 2: Check password using BCrypt
            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PaswdHash);

            if (!isValid)
            {
                return new UserDto
                {
                    Status = 0,
                    Message = "Invalid email or password"
                };
            }

            // Step 3: Success
            user.Status = 1;
            user.Message = "Login successful";

            return user;
        }
        catch (NpgsqlException)
        {
            return new UserDto { Status = 0, Message = "Database error occurred" };
        }
        catch (Exception)
        {
            return new UserDto { Status = 0, Message = "Application error occurred" };
        }
    }


    public async Task<UserDto> RegisterUserAsync(string email, string password, string venderCode)
    {
        try
        {
            // SP_RegisterUser(p_Email, p_Password, p_VenderCode)
            var result = await _repo.QueryFirstOrDefaultAsync<UserDto>(
                "_vender.SP_RegisterUser",
                new
                {
                    Email = email,
                    Password = password,
                    VenderCode = venderCode
                });

            if (result == null)
            {
                return new UserDto
                {
                    Status = 0,
                    Message = "No response from database"
                };
            }

            return result;
        }
        catch (NpgsqlException)
        {
            return new UserDto { Status = 0, Message = "Database error occurred" };
        }
        catch (Exception)
        {
            return new UserDto { Status = 0, Message = "Application error occurred" };
        }
    }
}
