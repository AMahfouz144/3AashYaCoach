using _3AashYaCoach.Models.Context;
using _3AashYaCoach.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _3AashYaCoach.Controllers
{
    

    [ApiController]
    [Route("api/Trainers")]
    public class TrainersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TrainersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetTrainers")]
        public async Task<IActionResult> GetAllTrainers()
        {
            var trainers = await _context.Users
                .Where(u => u.Role == UserRole.Coach)
                .Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.Email,
                    Certificates = u.TrainerProfile != null ? u.TrainerProfile.Certificates : null,
                    Experience = u.TrainerProfile != null ? u.TrainerProfile.ExperienceDetails : null,
                    ProfileImagePath = u.TrainerProfile != null ? u.TrainerProfile.ProfileImagePath : null,
                    Rate = u.TrainerProfile != null ? u.TrainerProfile.Rate : 0,
                })
                .ToListAsync();

            return Ok(trainers);
        }

       
       
        [HttpGet("GetSubscribers/{trinerId}")]
        public async Task<IActionResult> GetSubscribersForTrainer(Guid trinerId)
        {
            var coach = await _context.Users.FindAsync(trinerId);
            if (coach == null || coach.Role != UserRole.Coach)
                return NotFound("Coach not found or invalid.");

            var subscribers = await _context.Subscriptions
                .Where(s => s.CoachId == trinerId)
                .Include(s => s.Trainee)
                .Select(s => new
                {
                    TraineeId = s.TraineeId,
                    FullName = s.Trainee.FullName,
                    Email = s.Trainee.Email,
                    SubscribedAt = s.SubscribedAt,
                    Notes = s.Notes
                })
                .ToListAsync();

            var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);
            var recentCount = subscribers.Count(s => s.SubscribedAt >= oneMonthAgo);

            return Ok(new
            {
                TrainerId = trinerId,
                TrainerName = coach.FullName,
                SubscribersCount = subscribers.Count,
                RecentSubscribersCount = recentCount,
                Subscribers = subscribers
            });
        }


    }

}
