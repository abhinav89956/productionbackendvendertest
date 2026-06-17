using Npgsql;
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
            // SP_UserLogin(p_Email, p_Password)
            var result = await _repo.QueryFirstOrDefaultAsync<UserDto>(
                "_vender.SP_UserLogin",
                new
                {
                    Email = email,
                    Password = password
                });

            if (result == null)
            {
                return new UserDto
                {
                    Status = 0,
                    Message = "Invalid email or password"
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
