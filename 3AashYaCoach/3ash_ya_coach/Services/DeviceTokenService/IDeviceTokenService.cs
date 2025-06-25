namespace _3AashYaCoach._3ash_ya_coach.Services.DeviceTokenService
{
    public interface IDeviceTokenService
    {
        Task SaveOrUpdateTokenAsync(string userId, string token);
    }
}
