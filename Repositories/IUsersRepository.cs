using ABC.Models.Domain;
using ABC.Models.DTO;

namespace ABC.Repositories
{
    public interface IUsersRepository
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserInfo> GetUserByIdAsync(string userId);
        Task<UserInfo> AddUserAsync(RegisterRequestDto registerRequestDto, string AspNetUserId);
        Task<int> UpdateUserAsync(string id, UpdateUserDto user);
        Task<bool> DeleteUserAsync(string userId);
        Task<UserOptionsDto> Getoptions();
        Task<MailQueue> AddEmailRegisterAsync(string Email, string AspNetUserId);
        Task<UserProfile> AddUserProfileAsync(RegisterUserDto registerUserDto, string AspNetUserId);
        Task<MailQueue> AddEmailRegisterUserAsync(string Email, string AspNetUserId);
        Task<UserProfile> GetUserProfileAsync(string user_id);
        Task<UserAdminProfileDto> GetAdminProfileAsync(string user_id);
        Task<bool> UpdateUserAsync(UserUpdateModel model);
    }
}
