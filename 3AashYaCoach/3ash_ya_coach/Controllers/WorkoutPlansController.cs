using _3AashYaCoach._3ash_ya_coach.Dtos;
using _3AashYaCoach._3ash_ya_coach.Models;
using _3AashYaCoach.Models.Context;
using _3AashYaCoach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _3AashYaCoach.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using _3AashYaCoach._3ash_ya_coach.Services.PlanSubscriptionService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _3AashYaCoach._3ash_ya_coach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutPlansController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPlanSubscriptionService _subscriptionService;
        public WorkoutPlansController(AppDbContext context, IPlanSubscriptionService subscriptionService)
        {
            _context = context;
            _subscriptionService= subscriptionService;
        }
        [HttpPost("Subscribe")]
        //[Authorize(Roles = "Trainee")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeToPlanDto dto)
        {
            var result = await _subscriptionService.SubscribeAsync(dto);
            return result == null
                ? Ok(new { Message = "Subscribed successfully." })
                : BadRequest(new { Message = result });
        }

        [HttpDelete("Unsubscribe")]
        //[Authorize(Roles = "Trainee")]
        public async Task<IActionResult> Unsubscribe([FromBody] SubscribeToPlanDto dto)
        {
            var result = await _subscriptionService.UnsubscribeAsync(dto);
            return result == null
                ? Ok(new { Message = "Unsubscribed successfully." })
                : NotFound(new { Message = result });
        }

        [HttpGet("GetSubscribersCount/{planId}")]
        public async Task<IActionResult> GetSubscribersCount(Guid planId)
        {
            var count = await _subscriptionService.GetSubscribersCountAsync(planId);
            return Ok(new { PlanId = planId, SubscribersCount = count });
        }
        [HttpPost("CreateNewPlan")]
        public async Task<IActionResult> CreatePlan([FromBody] CreatePlanHeaderDto dto)
        {
            var coach = await _context.Users.FindAsync(dto.CoachId);
            if (coach == null || coach.Role != UserRole.Coach)
                return BadRequest("Invalid coach.");

            //if (dto.TraineeId.HasValue)
            //{
            //    var trainee = await _context.Users.FindAsync(dto.TraineeId.Value);
            //    if (trainee == null || trainee.Role != UserRole.Trainee)
            //        return BadRequest("Invalid trainee.");
            //}

            var plan = new WorkoutPlan
            {
                Id = Guid.NewGuid(),
                PlanName = dto.PlanName,
                PrimaryGoal = dto.PrimaryGoal,
                CoachId = dto.CoachId,
                CreatedAt = DateTime.UtcNow
            };

            _context.WorkoutPlans.Add(plan);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Plan created successfully." });
        }
        [HttpPut("UpdatePlan")]
        //[Authorize(Roles = "Coach")]
        public async Task<IActionResult> UpdateWorkoutPlan([FromBody] UpdateWorkoutPlanDto dto)
        {
            var plan = await _context.WorkoutPlans.FindAsync(dto.PlanId);
            if (plan == null)
                return NotFound("Workout plan not found.");

            // تحقق من أن المدرب الحالي هو نفسه صاحب الخطة
            //var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (plan.CoachId.ToString() != currentUserId)
            //    return Forbid("You are not authorized to update this plan.");

            plan.PlanName = dto.Title;
            plan.PrimaryGoal = dto.Description;

            _context.WorkoutPlans.Update(plan);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Workout plan updated successfully." });
        }


        [HttpPost("AddPlanDays/{planId}")]
        public async Task<IActionResult> AddDaysToPlan(Guid planId, [FromBody] List<WorkoutDayDto> daysDto)
        {
            var plan = await _context.WorkoutPlans.FindAsync(planId);
            if (plan == null)
                return NotFound("Workout plan not found.");

            foreach (var dayDto in daysDto)
            {
                var day = new WorkoutDay
                {
                    Id = Guid.NewGuid(),
                    WorkoutPlanId = planId,
                    DayNumber = dayDto.DayNumber,
                    DayName = dayDto.DayName,
                    Notes = dayDto.Notes,
                    Exercises = dayDto.Exercises.Select(e => new WorkoutExercise
                    {
                        Id = Guid.NewGuid(),
                        Name = e.Name,
                        muscleGroup = e.muscleGroup,
                        difficulty = e.difficulty,
                        Notes = e.Notes
                    }).ToList()
                };

                _context.WorkoutDays.Add(day);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Days added successfully." });
        }
        [HttpPut("EditPlanDays/{dayId}")]
        public async Task<IActionResult> UpdateWorkoutDay(Guid dayId, [FromBody] WorkoutDayDto dto)
        {
            var day = await _context.WorkoutDays
                .Include(d => d.Exercises)
                .FirstOrDefaultAsync(d => d.Id == dayId);

            if (day == null)
                return NotFound("Workout day not found.");

            // تحديث بيانات اليوم
            day.DayNumber = dto.DayNumber;
            day.DayName = dto.DayName;
            day.Notes = dto.Notes;

            // حذف التمارين القديمة
            _context.WorkoutExercises.RemoveRange(day.Exercises);

            // إضافة التمارين الجديدة
            day.Exercises = dto.Exercises.Select(e => new WorkoutExercise
            {
                Id = Guid.NewGuid(),
                Name = e.Name,
                difficulty = e.difficulty,
                muscleGroup = e.muscleGroup,
                Notes = e.Notes
            }).ToList();

            await _context.SaveChangesAsync();
            return Ok(new { message = "Workout day updated successfully." });
        }
        [HttpDelete("days/{dayId}")]
        public async Task<IActionResult> DeleteWorkoutDay(Guid dayId)
        {
            var day = await _context.WorkoutDays
                .Include(d => d.Exercises)
                .FirstOrDefaultAsync(d => d.Id == dayId);

            if (day == null)
                return NotFound("Workout day not found.");

            // حذف التمارين المرتبطة
            _context.WorkoutExercises.RemoveRange(day.Exercises);

            // حذف اليوم نفسه
            _context.WorkoutDays.Remove(day);

            await _context.SaveChangesAsync();
            return Ok(new { message = "Workout day deleted successfully." });
        }

        [HttpGet("GetPlanById/{planId}")]
        public async Task<IActionResult> GetPlanById(Guid planId)
        {
            var plan = await _context.WorkoutPlans
                .Include(p => p.Coach)
                .FirstOrDefaultAsync(p => p.Id == planId);

            if (plan == null)
                return NotFound("Plan not found.");

            return Ok(new
            {
                plan.Id,
                plan.PlanName,
                plan.PrimaryGoal,
                CoachId = plan.CoachId,
                CoachName = plan.Coach?.FullName,
                plan.CreatedAt
            });
        }

        [HttpGet("GetPublicPlans")]
        public async Task<IActionResult> GetPublicPlans()
        {
            var plans = await _context.WorkoutPlans
                .Include(p => p.Coach)
                .Select(p => new
                {
                    p.Id,
                    p.PlanName,
                    p.PrimaryGoal,
                    CoachName = p.Coach.FullName,
                    p.CreatedAt
                })
                .ToListAsync();

            return Ok(plans);
        }
        [HttpGet("GetPlansByCoach/{coachId}")]
        //[Authorize(Roles = "Coach,Admin")] // أو Admin فقط لو تحب
        public async Task<IActionResult> GetPlansByCoach(Guid coachId)
        {
            var coach = await _context.Users.FindAsync(coachId);
            if (coach == null || coach.Role != UserRole.Coach)
                return NotFound("Coach not found.");

            var plans = await _context.WorkoutPlans
                .Where(p => p.CoachId == coachId)
                .Include(p => p.Days)
                    .ThenInclude(d => d.Exercises)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new
                {
                    p.Id,
                    p.PlanName,
                    p.PrimaryGoal,
                    p.CreatedAt,
                    p.IsPublic,
                    Days = p.Days
                        .OrderBy(d => d.DayNumber)
                        .Select(d => new
                        {
                            d.Id,
                            d.DayNumber,
                            d.DayName,
                            d.Notes,
                            Exercises = d.Exercises.Select(e => new
                            {
                                e.Id,
                                e.Name,
                                e.muscleGroup,
                                e.difficulty,
                                e.Notes
                            })
                        })
                })
                .ToListAsync();

            return Ok(new
            {
                CoachId = coach.Id,
                CoachName = coach.FullName,
                Count = plans.Count,
                Plans = plans
            });
        }
    }
}
