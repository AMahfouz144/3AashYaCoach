using _3AashYaCoach.Dtos;
using _3AashYaCoach.Models;
using _3AashYaCoach.Models.Context;
using _3AashYaCoach.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _3AashYaCoach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavedCoachesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SavedCoachesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("SaveTrainers")]
        public async Task<IActionResult> SaveMultipleCoaches([FromBody] SaveCoachesDto dto)
        {
            // تحقق من وجود المتدرب
            var trainee = await _context.Users.FindAsync(dto.TraineeId);
            if (trainee == null || trainee.Role !=UserRole.Trainee)
                return BadRequest("Trainee not found or invalid.");

            var savedNow = new List<string>();
            var alreadySaved = new List<string>();

            foreach (var coachId in dto.CoachIds)
            {
                var coach = await _context.Users.FindAsync(coachId);
                if (coach == null || coach.Role != UserRole.Coach) continue;

                bool exists = await _context.SavedCoaches.AnyAsync(sc =>
                    sc.TraineeId == dto.TraineeId && sc.CoachId == coachId);

                if (exists)
                {
                    alreadySaved.Add(coach.FullName);
                    continue;
                }

                _context.SavedCoaches.Add(new SavedCoach
                {
                    Id = Guid.NewGuid(),
                    TraineeId = dto.TraineeId,
                    CoachId = coachId,
                    SavedAt = DateTime.UtcNow
                });

                savedNow.Add(coach.FullName);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Bulk save completed.",
                Saved = savedNow,
                Skipped = alreadySaved
            });
        }
        [HttpGet("GetSavedTrainers/{traineeId}")]
        public async Task<IActionResult> GetSavedCoaches(Guid traineeId)
        {
            // تحقق من وجود المتدرب
            var trainee = await _context.Users.FindAsync(traineeId);
            if (trainee == null || trainee.Role != UserRole.Trainee)
                return NotFound("Trainee not found or invalid.");

            // إحضار المدربين المحفوظين
            var savedCoaches = await _context.SavedCoaches
                .Where(sc => sc.TraineeId == traineeId)
                .Include(sc => sc.Coach)
                .Select(sc => new
                {
                    CoachId = sc.Coach.Id,
                    CoachName = sc.Coach.FullName,
                    CoachEmail = sc.Coach.Email,
                })
                .ToListAsync();

            return Ok(new
            {
                Count = savedCoaches.Count,
                Coaches = savedCoaches
            });
        }



    }
}
