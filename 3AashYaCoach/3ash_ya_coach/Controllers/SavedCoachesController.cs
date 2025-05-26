using _3AashYaCoach._3ash_ya_coach.Dtos;
using _3AashYaCoach._3ash_ya_coach.Services.SavedCoachService;
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
        private readonly ISavedCoachService _savedCoachService;
        public SavedCoachesController(AppDbContext context,ISavedCoachService savedCoachService)
        {
            _context = context;
            _savedCoachService = savedCoachService;
        }

        [HttpPost("SaveMultipleCoaches")]
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
            var result = await _savedCoachService.GetSavedCoachesDetailsAsync(traineeId);
            if (result == null)
                return NotFound("Trainee not found or invalid.");

            return Ok(result);
        }
        [HttpPost("SaveTrainer")]
        public async Task<IActionResult> SaveSingleCoach([FromBody] SaveSingleCoachDto dto)
        {
            var result = await _savedCoachService.SaveSingleCoachAsync(dto);

            if (result == null)
                return Ok(new { Message = "Coach saved successfully." });

            return BadRequest(new { Message = result });
        }

        [HttpDelete("UnsaveTrainer")]
        public async Task<IActionResult> UnsaveCoach([FromBody] UnsaveCoachDto dto)
        {
            var result = await _savedCoachService.UnsaveSingleCoachAsync(dto);

            if (result == null)
                return Ok(new { Message = "Coach unsaved successfully." });

            return BadRequest(new { Message = result });
        }



    }
}
