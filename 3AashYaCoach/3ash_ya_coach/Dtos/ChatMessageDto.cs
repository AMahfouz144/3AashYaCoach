namespace _3AashYaCoach._3ash_ya_coach.Dtos
{
    public class ChatMessageDto
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
