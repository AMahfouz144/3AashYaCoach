using Microsoft.AspNetCore.Mvc;
using _3AashYaCoach.Models.Context;
using _3AashYaCoach._3ash_ya_coach.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace _3AashYaCoach._3ash_ya_coach.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TraineeController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TraineeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{traineeId}")]
        public async Task<ActionResult<TraineeDetailDto>> GetTraineeDetail(Guid traineeId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == traineeId);
            if (user == null)
                return NotFound("Trainee not found.");

            // Get all plans the trainee is subscribed to
            var planSubs = await _context.PlanSubscriptions
                .Where(ps => ps.TraineeId == traineeId)
                .Include(ps => ps.Plan)
                    .ThenInclude(p => p.Coach)
                .ToListAsync();

            var plans = planSubs.Select(ps => ps.Plan).Distinct().ToList();

            var planProgress = new List<TraineePlanProgressDto>();
            foreach (var plan in plans)
            {
                var days = await _context.WorkoutDays
                    .Where(d => d.WorkoutPlanId == plan.Id)
                    .ToListAsync();
                planProgress.Add(new TraineePlanProgressDto
                {
                    PlanId = plan.Id,
                    PlanName = plan.PlanName,
                    PrimaryGoal = plan.PrimaryGoal,
                    CoachId = plan.CoachId,
                    CoachName = plan.Coach?.FullName ?? string.Empty,
                    TotalDays = days.Count,
                    CompletedDays = days.Count(d => d.IsCompleted)
                });
            }

            var dto = new TraineeDetailDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                SubscribedPlans = planProgress
            };
            return Ok(dto);
        }
    }
} 