using VenderTest.DTOs;

namespace VenderTest.Repository
{
    public interface IUserRepository
    {
        Task<UserDto> RegisterUserAsync(string email, string password, string venderCode);
        Task<UserDto> LoginAsync(string email, string password);
    }
}
