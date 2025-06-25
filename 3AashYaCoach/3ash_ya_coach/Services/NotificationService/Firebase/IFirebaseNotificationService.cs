namespace _3AashYaCoach._3ash_ya_coach.Services.NotificationService.Firebase
{
    public interface IFirebaseNotificationService
    {
        Task SendNotificationAsync(string token, string title, string body);
    }
}
