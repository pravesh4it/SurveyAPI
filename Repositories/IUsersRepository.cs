using ABC.Models.Domain;
using ABC.Models.DTO;

namespace ABC.Repositories
{
    public interface IUsersRepository
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserInfo> GetUserByIdAsync(string userId);
        Task<UserInfo> AddUserAsync(RegisterRequestDto registerRequestDto, string AspNetUserId);
        Task<UserInfo> UpdateUserAsync(UserInfo user);
        Task<bool> DeleteUserAsync(string userId);
        Task<UserOptionsDto> Getoptions();
        Task<string> AddEmailRegisterAsync(string Email, string AspNetUserId);
        Task<UserProfile> AddUserProfileAsync(RegisterUserDto registerUserDto, string AspNetUserId);
        Task<MailQueue> AddEmailRegisterUserAsync(string Email, string AspNetUserId);
        Task<UserProfile> GetUserProfileAsync(string user_id);
    }
}
