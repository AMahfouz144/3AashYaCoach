using _3AashYaCoach._3ash_ya_coach.Dtos;

namespace _3AashYaCoach._3ash_ya_coach.Services.PlanSubscriptionService
{
    public interface IPlanSubscriptionService
    {
        Task<string?> SubscribeAsync(SubscribeToPlanDto dto);
        Task<string?> UnsubscribeAsync(SubscribeToPlanDto dto);
        Task<int> GetSubscribersCountAsync(Guid planId);
    }
}
