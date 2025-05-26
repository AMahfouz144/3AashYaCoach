using _3AashYaCoach._3ash_ya_coach.Dtos;
using _3AashYaCoach.Dtos;

namespace _3AashYaCoach._3ash_ya_coach.Services.SavedCoachService
{
    public interface ISavedCoachService
    {
        Task<List<SavedTrainerDto>> GetSavedCoachesDetailsAsync(Guid traineeId);
        Task<string?> SaveSingleCoachAsync(SaveSingleCoachDto dto);
        Task<string?> UnsaveSingleCoachAsync(UnsaveCoachDto dto);
    }
}
