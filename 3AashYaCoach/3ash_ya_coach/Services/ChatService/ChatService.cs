
using _3AashYaCoach._3ash_ya_coach.Models;
using _3AashYaCoach.Models.Context;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace _3AashYaCoach._3ash_ya_coach.Services
{
    public class ChatService:IChatService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatService(AppDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<ChatMessage> SaveMessageAsync(Guid senderId, Guid receiverId, string text)
        {
            var msg = new ChatMessage
            {
                Id = Guid.NewGuid(),
                SenderId = senderId,
                ReceiverId = receiverId,
                Text = text
            };

            //_context.ChatMessages.Add(msg);
            _context.ChatMessages.Add(msg);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, text);
            await _hubContext.Clients.User(senderId.ToString()).SendAsync("ReceiveMessage", senderId, text);

            return msg;
        }
        public async Task<List<ChatMessage>> GetMessagesAsync(Guid senderId, Guid receiverId)
        {
            return await _context.ChatMessages
                .Where(m =>
                    (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                    (m.SenderId == receiverId && m.ReceiverId == senderId))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

    }
}
