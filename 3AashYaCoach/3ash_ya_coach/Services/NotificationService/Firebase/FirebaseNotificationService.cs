
using FirebaseAdmin.Messaging;

namespace _3AashYaCoach._3ash_ya_coach.Services.NotificationService.Firebase
{
    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        public async Task SendNotificationAsync(string token, string title, string body)
        {
            var message = new Message()
            {
                Token = token,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High
                }
            };

            await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }

}
