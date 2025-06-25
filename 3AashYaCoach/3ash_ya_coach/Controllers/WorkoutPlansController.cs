using _3AashYaCoach._3ash_ya_coach.Dtos;
using _3AashYaCoach._3ash_ya_coach.Models;
using _3AashYaCoach.Models.Context;
using _3AashYaCoach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _3AashYaCoach.Models.Enums;
using _3AashYaCoach._3ash_ya_coach.Services.PlanSubscriptionService;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using _3AashYaCoach._3ash_ya_coach.Shared;
using _3AashYaCoach._3ash_ya_coach.Services.WorkoutPlanService;

namespace _3AashYaCoach._3ash_ya_coach.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutPlansController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPlanSubscriptionService _subscriptionService;
        private readonly IWorkoutPlanService _workoutPlanService;
        public WorkoutPlansController(AppDbContext context, IPlanSubscriptionService subscriptionService, IWorkoutPlanService _workoutPlanService)
        {
            _context = context;
            this._workoutPlanService = _workoutPlanService;
            _subscriptionService = subscriptionService;
        }
        [HttpPost("Subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeToPlanDto dto)
        {
            var result = await _subscriptionService.SubscribeAsync(dto);
            return result == null
                ? Ok(new { Message = "Subscribed successfully." })
                : BadRequest(new { Message = result });
        }

        [HttpDelete("Unsubscribe")]
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
        public async Task<IActionResult> UpdateWorkoutPlan([FromBody] UpdateWorkoutPlanDto dto)
        {
            var plan = await _context.WorkoutPlans.FindAsync(dto.PlanId);
            if (plan == null)
                return NotFound("Workout plan not found.");

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (plan.CoachId.ToString() != currentUserId)
                return Forbid("You are not authorized to update this plan.");

            plan.PlanName = dto.PlanName;
            plan.PrimaryGoal = dto.PrimaryGoal;

            _context.WorkoutPlans.Update(plan);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Workout plan updated successfully." });
        }
        [HttpPut("UpdatePlanById/{planId}")]
        public async Task<IActionResult> UpdatePlanById(Guid planId, [FromBody] UpdateWorkoutPlanDto dto)
        {
            var plan = await _context.WorkoutPlans
                .Include(p => p.Days)
                    .ThenInclude(d => d.Exercises)
                .FirstOrDefaultAsync(p => p.Id == planId);

            if (plan == null)
                return NotFound("Workout plan not found.");

            plan.PlanName = dto.PlanName;
            plan.PrimaryGoal = dto.PrimaryGoal;

            foreach (var dayDto in dto.Days)
            {
                var existingDay = plan.Days
                    .FirstOrDefault(d => d.DayNumber == dayDto.DayNumber && d.WorkoutPlanId == planId);

                if (existingDay != null)
                {
                    existingDay.DayName = dayDto.DayName;
                    existingDay.Notes = dayDto.Notes;

                    var updatedExerciseIds = dayDto.Exercises.Where(e => e.Id.HasValue).Select(e => e.Id.Value).ToList();
                    var exercisesToRemove = existingDay.Exercises.Where(e => !updatedExerciseIds.Contains(e.Id)).ToList();
                    _context.WorkoutExercises.RemoveRange(exercisesToRemove);

                    foreach (var exDto in dayDto.Exercises)
                    {
                        if (exDto.Id.HasValue)
                        {
                            var existingEx = existingDay.Exercises.FirstOrDefault(e => e.Id == exDto.Id.Value);
                            if (existingEx != null)
                            {
                                existingEx.Name = exDto.Name;
                                existingEx.difficulty = exDto.difficulty;
                                existingEx.muscleGroup = exDto.muscleGroup;
                                existingEx.Notes = exDto.Notes;
                            }
                        }
                        else
                        {
                            existingDay.Exercises.Add(new WorkoutExercise
                            {
                                Id = Guid.NewGuid(),
                                WorkoutDayId = existingDay.Id,
                                Name = exDto.Name,
                                difficulty = exDto.difficulty,
                                muscleGroup = exDto.muscleGroup,
                                Notes = exDto.Notes
                            });
                        }
                    }
                }
                else
                {
                    var newDay = new WorkoutDay
                    {
                        Id = Guid.NewGuid(),
                        WorkoutPlanId = planId,
                        DayNumber = dayDto.DayNumber,
                        DayName = dayDto.DayName,
                        Notes = dayDto.Notes,
                        Exercises = dayDto.Exercises.Select(exDto => new WorkoutExercise
                        {
                            Id = Guid.NewGuid(),
                            Name = exDto.Name,
                            difficulty = exDto.difficulty,
                            muscleGroup = exDto.muscleGroup,
                            Notes = exDto.Notes
                        }).ToList()
                    };
                    _context.WorkoutDays.Add(newDay);
                }
            }

            var incomingDayNumbers = dto.Days.Select(d => d.DayNumber).ToList();
            var daysToRemove = plan.Days.Where(d => !incomingDayNumbers.Contains(d.DayNumber)).ToList();
            _context.WorkoutDays.RemoveRange(daysToRemove);

            await _context.SaveChangesAsync();
            return Ok(new { message = "Workout plan, days, and exercises updated successfully." });
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
      
        [HttpDelete("DeletePlan/{planId}")]
        public async Task<IActionResult> DeletePlan(Guid planId)
        {
            var plan = await _context.WorkoutPlans
                .Include(p => p.Days)
                    .ThenInclude(d => d.Exercises)
                .FirstOrDefaultAsync(p => p.Id == planId);

            if (plan == null)
                return NotFound(new ErrorResponse("PlanNotFound", "Workout plan not found."));

            // حذف التمارين أولًا
            foreach (var day in plan.Days)
            {
                _context.WorkoutExercises.RemoveRange(day.Exercises);
            }

            // حذف الأيام
            _context.WorkoutDays.RemoveRange(plan.Days);

            // حذف الخطة
            _context.WorkoutPlans.Remove(plan);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Workout plan deleted successfully." });
        }

        [HttpGet("GetPlanById/{planId}")]
        public async Task<IActionResult> GetPlanById(Guid planId)
        {
            var plan = await _context.WorkoutPlans
                .Include(p => p.Coach)
                .Include(p => p.Days)
                    .ThenInclude(d => d.Exercises)
                .FirstOrDefaultAsync(p => p.Id == planId);

            if (plan == null)
                return NotFound("Plan not found.");

            var result = new
            {
                plan.Id,
                plan.PlanName,
                plan.PrimaryGoal,
                CoachId = plan.CoachId,
                CoachName = plan.Coach?.FullName,
                plan.CreatedAt,
                Days = plan.Days
                    .OrderBy(d => d.DayNumber)
                    .Select(d => new
                    {
                        d.Id,
                        d.DayNumber,
                        d.DayName,
                        d.Notes,
                        d.IsCompleted,
                        Exercises = d.Exercises.Select(e => new
                        {
                            e.Id,
                            e.Name,
                            e.muscleGroup,
                            e.difficulty,
                            e.Notes
                        })
                    })
            };

            return Ok(result);
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
                            d.IsCompleted,
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
        [HttpPost("GetPlansByCoachIds")]
        public async Task<IActionResult> GetPlansByCoachIds([FromBody] CoachPlansRequestDto dto)
        {
            if (dto.CoachIds == null || dto.CoachIds.Count == 0)
                return BadRequest(new ErrorResponse("InvalidRequest", "No coach IDs provided."));

            var plans = await _workoutPlanService.GetPlansByCoachIdsAsync(dto.CoachIds, dto.TraineeId);
            return Ok(plans);
        }



        // ===========================================================================================================//
        [HttpPost("CreateDay/{planId}")]
        public async Task<IActionResult> CreateDay(Guid planId, [FromBody] WorkoutDayDto dto)
        {
            var plan = await _context.WorkoutPlans.FindAsync(planId);
            if (plan == null)
                return NotFound("Workout plan not found.");

            var newDay = new WorkoutDay
            {
                Id = Guid.NewGuid(),
                WorkoutPlanId = planId,
                DayNumber = dto.DayNumber,
                DayName = dto.DayName,
                Notes = dto.Notes,
                Exercises = dto.Exercises?.Select(e => new WorkoutExercise
                {
                    Id = Guid.NewGuid(),
                    Name = e.Name,
                    difficulty = e.difficulty,
                    muscleGroup = e.muscleGroup,
                    Notes = e.Notes
                }).ToList()
            };

            _context.WorkoutDays.Add(newDay);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Workout day created successfully.", DayId = newDay.Id });
        }
        [HttpGet("GetDayById/{dayId}")]
        public async Task<IActionResult> GetDayById(Guid dayId)
        {
            var day = await _context.WorkoutDays
                .Include(d => d.Exercises)
                .FirstOrDefaultAsync(d => d.Id == dayId);

            if (day == null)
                return NotFound("Workout day not found.");

            return Ok(new
            {
                day.Id,
                day.DayNumber,
                day.DayName,
                day.Notes,
                Exercises = day.Exercises.Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.difficulty,
                    e.muscleGroup,
                    e.VideoLink,
                    e.IsCompleted,
                    e.Notes
                })
            });
        }
        [HttpPut("UpdateDay/{dayId}")]
        public async Task<IActionResult> UpdateDay(Guid dayId, [FromBody] WorkoutDayDto dto)
        {
            var day = await _context.WorkoutDays.FindAsync(dayId);
            if (day == null)
                return NotFound("Workout day not found.");

            day.DayNumber = dto.DayNumber;
            day.DayName = dto.DayName;
            day.Notes = dto.Notes;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Workout day updated successfully." });
        }
        [HttpDelete("DeleteDay/{dayId}")]
        public async Task<IActionResult> DeleteDay(Guid dayId)
        {
            var day = await _context.WorkoutDays
                .Include(d => d.Exercises)
                .FirstOrDefaultAsync(d => d.Id == dayId);

            if (day == null)
                return NotFound("Workout day not found.");

            _context.WorkoutExercises.RemoveRange(day.Exercises);
            _context.WorkoutDays.Remove(day);

            await _context.SaveChangesAsync();
            return Ok(new { message = "Workout day deleted successfully." });
        }
        [HttpPut("SetDayCompleted/{dayId}")]
        public async Task<IActionResult> SetDayCompleted(Guid dayId, [FromQuery] bool isCompleted)
        {
            var day = await _context.WorkoutDays.FindAsync(dayId);
            if (day == null)
                return NotFound(new { Message = "Workout day not found." });

            day.IsCompleted = isCompleted;
            _context.WorkoutDays.Update(day);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Workout day marked as {(isCompleted ? "completed" : "not completed")}." });
        }

        [HttpPut("ToggleDayCompleted/{dayId}")]
        public async Task<IActionResult> ToggleDayCompleted(Guid dayId)
        {
            var day = await _context.WorkoutDays.FindAsync(dayId);
            if (day == null)
                return NotFound(new { Message = "Workout day not found." });

            // Toggle the completion status
            day.IsCompleted = !day.IsCompleted;
            
            _context.WorkoutDays.Update(day);
            await _context.SaveChangesAsync();

            return Ok(new { 
                Message = $"Workout day marked as {(day.IsCompleted ? "completed" : "not completed")}.",
                DayId = day.Id,
                IsCompleted = day.IsCompleted
            });
        }

    }
}