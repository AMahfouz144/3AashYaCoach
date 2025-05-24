using _3AashYaCoach._3ash_ya_coach.Models;

namespace _3AashYaCoach._3ash_ya_coach.Services
{
    public interface IChatService
    {
            Task<ChatMessage> SaveMessageAsync(Guid senderId, Guid receiverId, string text);
        Task<List<ChatMessage>> GetMessagesAsync(Guid senderId, Guid receiverId);

    }
}
