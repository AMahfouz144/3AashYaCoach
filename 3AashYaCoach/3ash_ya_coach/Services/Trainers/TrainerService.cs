using _3AashYaCoach._3ash_ya_coach.Dtos;
using _3AashYaCoach.Migrations;
using _3AashYaCoach.Models.Context;
using _3AashYaCoach.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace _3AashYaCoach._3ash_ya_coach.Services.Trainers
{
    public class TrainerService : ITrainerService
    {
        private readonly AppDbContext _context;

        public TrainerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TrainerDto>> GetAllTrainersWithSubscriptionStatus(Guid traineeId)
        {
            var subscribedCoachIds = await _context.Subscriptions
                .Where(s => s.TraineeId == traineeId)
                .Select(s => s.CoachId)
                .ToListAsync();
            var savedCoachs = await _context.SavedCoaches
                .Where(s => s.TraineeId == traineeId)
                .Select(s => s.CoachId)
                .ToListAsync();
            var trainers = await _context.Users
                .Where(u => u.Role == UserRole.Coach)
                .Include(u => u.TrainerProfile)
                .Select(u => new TrainerDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Certificates = u.TrainerProfile != null ? u.TrainerProfile.Certificates : null,
                    Experience = u.TrainerProfile != null ? u.TrainerProfile.ExperienceDetails : null,
                    ProfileImagePath = u.TrainerProfile != null ? u.TrainerProfile.ProfileImagePath : null,
                    Rate = u.TrainerProfile != null ? u.TrainerProfile.Rate : 0,
                    IsSubscribed = subscribedCoachIds.Contains(u.Id),
                    IsBookMarked= savedCoachs.Contains(u.Id)
                })
                .ToListAsync();

            return trainers;
        }
    }

}
