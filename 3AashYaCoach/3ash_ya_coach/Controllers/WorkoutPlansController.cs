using _3AashYaCoach._3ash_ya_coach.Dtos;
using _3AashYaCoach._3ash_ya_coach.Models;
using _3AashYaCoach.Models.Context;
using _3AashYaCoach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _3AashYaCoach._3ash_ya_coach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutPlansController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WorkoutPlansController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("CreateNewPlan")]
        public async Task<IActionResult> CreatePlan([FromBody] CreatePlanHeaderDto dto)
        {
            var plan = new WorkoutPlan
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                CoachId = dto.CoachId,
                TraineeId = dto.TraineeId,
                CreatedAt = DateTime.UtcNow
            };

            _context.WorkoutPlans.Add(plan);
            await _context.SaveChangesAsync();

            return Ok(new { plan.Id, message = "Workout plan created." });
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
                        Sets = e.Sets,
                        Reps = e.Reps,
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
                Sets = e.Sets,
                Reps = e.Reps,
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

        [HttpGet("GetPlans/{planId}")]
        public async Task<IActionResult> GetFullWorkoutPlan(Guid planId)
        {
            var plan = await _context.WorkoutPlans
                .Include(p => p.Coach)
                .Include(p => p.Trainee)
                .Include(p => p.Days)
                    .ThenInclude(d => d.Exercises)
                .FirstOrDefaultAsync(p => p.Id == planId);

            if (plan == null)
                return NotFound("Workout plan not found.");

            var result = new
            {
                plan.Id,
                plan.Title,
                plan.Description,
                plan.CreatedAt,
                Coach = new
                {
                    plan.CoachId,
                    plan.Coach.FullName
                },
                Trainee = new
                {
                    plan.TraineeId,
                    plan.Trainee.FullName
                },
                Days = plan.Days.Select(d => new
                {
                    d.DayNumber,
                    d.DayName,
                    d.Notes,
                    Exercises = d.Exercises.Select(e => new
                    {
                        e.Name,
                        e.Sets,
                        e.Reps,
                        e.Notes
                    })
                })
            };

            return Ok(result);
        }

    }
}
