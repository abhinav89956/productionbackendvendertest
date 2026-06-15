using VenderTest.DTOs;

namespace VenderTest.Service
{
    public interface IUserService
    {
        Task<UserDto> RegisterUserAsync(string email, string password, string venderCode);
        Task<UserDto?> LoginAsync(string email, string password);
    }
}
