using _3AashYaCoach._3ash_ya_coach.Dtos;
using _3AashYaCoach.Models;

namespace _3AashYaCoach._3ash_ya_coach.Services.WorkoutPlanService
{
    public interface IWorkoutPlanService
    {
        Task<string?> UpdatePlanWithDaysAndExercises(UpdateWorkoutPlanDto dto);
        Task<string?> SubscribeToPlan(SubscribeToPlanDto dto);
        Task<string?> UnsubscribeFromPlan(SubscribeToPlanDto dto);
        Task<int> GetSubscribersCount(Guid planId);
        Task<string?> CreatePlan(CreatePlanHeaderDto dto);
        Task<WorkoutPlanBriefDto?> GetPlanById(Guid planId);
        Task<List<WorkoutPlanBriefDto>> GetPlansByCoachIdsAsync(List<Guid> coachIds, Guid traineeId);

    }
}
