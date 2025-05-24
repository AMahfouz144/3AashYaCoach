using Microsoft.AspNetCore.SignalR;

namespace _3AashYaCoach._3ash_ya_coach.Services
{
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            // أي شيء إضافي عند الاتصال
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // أي شيء إضافي عند فصل الاتصال
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(Guid senderId, Guid receiverId, string text)
        {
            await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, text);
        }
    }

}
