using _3AashYaCoach._3ash_ya_coach.Dtos;

namespace _3AashYaCoach._3ash_ya_coach.Services.SubscriptionService
{
    public class SubscribersResultDto
    {
        public Guid TrainerId { get; set; }
        public string TrainerName { get; set; } = string.Empty;
        public int SubscribersCount { get; set; }
        public int RecentSubscribersCount { get; set; }
        public List<SubscriberDto> Subscribers { get; set; } = new();
    }

}
