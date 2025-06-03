using _3AashYaCoach._3ash_ya_coach.Dtos;

namespace _3AashYaCoach._3ash_ya_coach.Services.UserInfoService
{
    public interface IUserInfoService
    {
        Task<UserInfoDto> GetUserInfoAsync(Guid userId);
    }
}
