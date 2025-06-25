namespace _3AashYaCoach._3ash_ya_coach.Services.SubscriptionService
{
    public interface ISubscriptionService
    {
        Task<SubscribersResultDto?> GetSubscribersForTrainerAsync(Guid trainerId);

    }
}
