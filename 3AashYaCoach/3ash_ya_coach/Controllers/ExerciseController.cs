using _3AashYaCoach._3ash_ya_coach.Dtos;
using _3AashYaCoach._3ash_ya_coach.Models;
using _3AashYaCoach.Models.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _3AashYaCoach._3ash_ya_coach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ExerciseController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPut("EditExercise/{exerciseId}")]
        public async Task<IActionResult> EditExercise(Guid exerciseId, [FromBody] WorkoutExerciseDto dto)
        {
            var exercise = await _context.WorkoutExercises.FindAsync(exerciseId);
            if (exercise == null)
                return NotFound("Exercise not found.");

            exercise.Name = dto.Name;
            exercise.difficulty = dto.difficulty;
            exercise.muscleGroup = dto.muscleGroup;
            exercise.VideoLink = dto.VideoLink;
            exercise.Notes = dto.Notes;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Exercise updated successfully." });
        }
        [HttpPost("AddExerciseToDay/{dayId}")]
        public async Task<IActionResult> AddExerciseToDay(Guid dayId, [FromBody] WorkoutExerciseDto dto)
        {
            var day = await _context.WorkoutDays.FindAsync(dayId);
            if (day == null)
                return NotFound("Workout day not found.");

            var exercise = new WorkoutExercise
            {
                Id = Guid.NewGuid(),
                WorkoutDayId = day.Id,
                Name = dto.Name,
                difficulty = dto.difficulty,
                muscleGroup = dto.muscleGroup,
                VideoLink = dto.VideoLink,
                Notes = dto.Notes
            };

            _context.WorkoutExercises.Add(exercise);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Exercise added successfully to the day." });
        }

        [HttpDelete("DeleteExercise/{exerciseId}")]
        public async Task<IActionResult> DeleteExercise(Guid exerciseId)
        {
            var exercise = await _context.WorkoutExercises.FindAsync(exerciseId);
            if (exercise == null)
                return NotFound("Exercise not found.");

            _context.WorkoutExercises.Remove(exercise);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Exercise deleted successfully." });
        }
        [HttpGet("GetExercisesByDay/{dayId}")]
        public async Task<IActionResult> GetExercisesByDay(Guid dayId)
        {
            var day = await _context.WorkoutDays
                .Include(d => d.Exercises)
                .FirstOrDefaultAsync(d => d.Id == dayId);

            if (day == null)
                return NotFound("Workout day not found.");

            var exercises = day.Exercises.Select(e => new
            {
                e.Id,
                e.Name,
                e.difficulty,
                e.muscleGroup,
                e.VideoLink,
                e.IsCompleted,
                e.Notes
            }).ToList();

            return Ok(new
            {
                DayId = day.Id,
                DayName = day.DayName,
                Exercises = exercises
            });
        }

        [HttpPut("ToggleExerciseCompleted/{exerciseId}")]
        public async Task<IActionResult> ToggleExerciseCompleted(Guid exerciseId)
        {
            var exercise = await _context.WorkoutExercises.FindAsync(exerciseId);
            if (exercise == null)
                return NotFound(new { Message = "Exercise not found." });

            // Toggle the completion status
            exercise.IsCompleted = !exercise.IsCompleted;
            
            _context.WorkoutExercises.Update(exercise);
            await _context.SaveChangesAsync();

            return Ok(new { 
                Message = $"Exercise marked as {(exercise.IsCompleted ? "completed" : "not completed")}.",
                ExerciseId = exercise.Id,
                IsCompleted = exercise.IsCompleted
            });
        }

    }
}
