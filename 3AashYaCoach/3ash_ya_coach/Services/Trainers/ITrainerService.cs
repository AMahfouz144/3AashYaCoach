using _3AashYaCoach._3ash_ya_coach.Dtos;

namespace _3AashYaCoach._3ash_ya_coach.Services.Trainers
{
    public interface ITrainerService
    {
        Task<List<TrainerDto>> GetAllTrainersWithSubscriptionStatus(Guid traineeId);
    }
}
