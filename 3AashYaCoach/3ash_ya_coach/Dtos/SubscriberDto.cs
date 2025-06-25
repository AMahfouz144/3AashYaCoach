namespace _3AashYaCoach._3ash_ya_coach.Dtos
{
    public class SubscriberDto
    {
        public Guid TraineeId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime SubscribedAt { get; set; }
        public string? Notes { get; set; }
    }

}
