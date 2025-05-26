using _3AashYaCoach._3ash_ya_coach.Dtos;
using _3AashYaCoach.Dtos;
using _3AashYaCoach.Models;
using _3AashYaCoach.Models.Context;
using _3AashYaCoach.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace _3AashYaCoach._3ash_ya_coach.Services.SavedCoachService
{
    public class SavedCoachService : ISavedCoachService
    {
        private readonly AppDbContext _context;

        public SavedCoachService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<string?> SaveSingleCoachAsync(SaveSingleCoachDto dto)
        {
            var trainee = await _context.Users.FindAsync(dto.TraineeId);
            if (trainee == null || trainee.Role != UserRole.Trainee)
                return "Trainee not found or invalid.";

            var coach = await _context.Users.FindAsync(dto.CoachId);
            if (coach == null || coach.Role != UserRole.Coach)
                return "Coach not found or invalid.";

            var exists = await _context.SavedCoaches.AnyAsync(sc =>
                sc.TraineeId == dto.TraineeId && sc.CoachId == dto.CoachId);

            if (exists)
                return "This coach is already saved.";

            var savedCoach = new SavedCoach
            {
                Id = Guid.NewGuid(),
                TraineeId = dto.TraineeId,
                CoachId = dto.CoachId,
                SavedAt = DateTime.UtcNow
            };

            _context.SavedCoaches.Add(savedCoach);
            await _context.SaveChangesAsync();

            return null; 
        }
        public async Task<List<SavedTrainerDto>> GetSavedCoachesDetailsAsync(Guid traineeId)
        {
            var trainee = await _context.Users.FindAsync(traineeId);
            if (trainee == null || trainee.Role != UserRole.Trainee)
                return null!;

            var savedCoaches = await _context.SavedCoaches
                .Where(sc => sc.TraineeId == traineeId)
                .Include(sc => sc.Coach)
                .ThenInclude(c => c.TrainerProfile)
                .Select(sc => new SavedTrainerDto
                {
                    Id = sc.Coach.Id,
                    FullName = sc.Coach.FullName,
                    Email = sc.Coach.Email,
                    Certificates = sc.Coach.TrainerProfile != null ? sc.Coach.TrainerProfile.Certificates : null,
                    ExperienceDetails = sc.Coach.TrainerProfile != null ? sc.Coach.TrainerProfile.ExperienceDetails : null,
                    ProfileImagePath = sc.Coach.TrainerProfile != null ? sc.Coach.TrainerProfile.ProfileImagePath : null,
                    Rate = sc.Coach.TrainerProfile != null ? sc.Coach.TrainerProfile.Rate : 0
                })
                .ToListAsync();

            return savedCoaches;
        }
        public async Task<string?> UnsaveSingleCoachAsync(UnsaveCoachDto dto)
        {
            var trainee = await _context.Users.FindAsync(dto.TraineeId);
            if (trainee == null || trainee.Role != UserRole.Trainee)
                return "Trainee not found or invalid.";

            var coach = await _context.Users.FindAsync(dto.CoachId);
            if (coach == null || coach.Role != UserRole.Coach)
                return "Coach not found or invalid.";

            var savedCoach = await _context.SavedCoaches
                .FirstOrDefaultAsync(sc => sc.TraineeId == dto.TraineeId && sc.CoachId == dto.CoachId);

            if (savedCoach == null)
                return "Coach is not saved by this trainee.";

            _context.SavedCoaches.Remove(savedCoach);
            await _context.SaveChangesAsync();

            return null; // null = success
        }

    }

}
