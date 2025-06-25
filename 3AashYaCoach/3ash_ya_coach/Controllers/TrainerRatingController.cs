using _3AashYaCoach._3ash_ya_coach.Models;
using _3AashYaCoach.Models.Context;
using _3AashYaCoach.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using _3AashYaCoach._3ash_ya_coach.Dtos;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class TrainerRatingController : ControllerBase
{
    private readonly AppDbContext _context;

    public TrainerRatingController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("rate")]
    public async Task<IActionResult> RateTrainer([FromBody] CreateTrainerRatingDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var existingRating = await _context.TrainerRatings
            .FirstOrDefaultAsync(r => r.CoachId == dto.CoachId && r.RatedByUserId == Guid.Parse(userId));

        if (existingRating != null)
        {
            existingRating.RatingValue = dto.RatingValue;
            existingRating.CreatedAt = DateTime.UtcNow;
        }
        else
        {
            var rating = new TrainerRating
            {
                CoachId = dto.CoachId,
                RatedByUserId = Guid.Parse(userId),
                RatingValue = dto.RatingValue
            };
            _context.TrainerRatings.Add(rating);
        }

        await _context.SaveChangesAsync();
        await UpdateCoachAverageRate(dto.CoachId);

        return Ok("Rating submitted");
    }

    private async Task UpdateCoachAverageRate(Guid coachId)
    {
        var ratings = await _context.TrainerRatings
            .Where(r => r.CoachId == coachId)
            .ToListAsync();

        if (ratings.Any())
        {
            var average = Math.Round(ratings.Average(r => r.RatingValue), 2);
            var coach = await _context.Trainers.FindAsync(coachId);
            if (coach != null)
            {
                coach.Rate = average;
                await _context.SaveChangesAsync();
            }
        }
    }

    [HttpGet("get-rating/{coachId}")]
    public async Task<IActionResult> GetRating(Guid coachId)
    {
        var coach = await _context.Trainers.FindAsync(coachId);
        if (coach == null) return NotFound("Coach not found");

        return Ok(new { CoachId = coachId, AverageRate = coach.Rate });
    }
}
