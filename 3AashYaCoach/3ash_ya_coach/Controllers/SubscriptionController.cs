using _3AashYaCoach.Models.Context;
using _3AashYaCoach.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using _3AashYaCoach.Dtos;
using _3AashYaCoach.Models.Enums;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SubscriptionsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("subscibe")]
    //[Authorize(Roles = "Trainee")]
    public async Task<IActionResult> Subscribe([FromBody] CreateSubscriptionDto dto)
    {
        // التحقق من صحة المستخدم الموجود
        var trainee = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.TraineeId && u.Role == UserRole.Trainee);
        if (trainee == null)
            return NotFound("Coach not found.");

        var coach = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.CoachId && u.Role == UserRole.Coach);
        if (coach == null)
            return NotFound("Coach not found.");

        bool alreadySubscribed = await _context.Subscriptions.AnyAsync(s => s.TraineeId == dto.TraineeId && s.CoachId == dto.CoachId);
        if (alreadySubscribed)
            return BadRequest("This trainee is already subscribed to this coach.");

        var subscription = new Subscription
        {
            Id = Guid.NewGuid(),
            TraineeId = dto.TraineeId,
            CoachId = dto.CoachId,
            SubscribedAt = DateTime.UtcNow
        };

        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Subscription created successfully." });
    }

    [HttpDelete("unsubscibe")]
    //[Authorize(Roles = "Trainee")]
    public async Task<IActionResult> Unsubscribe([FromBody] UnsubscribeDto dto)
    {
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s =>
                s.TraineeId == dto.TraineeId &&
                s.CoachId == dto.CoachId);

        if (subscription == null)
            return NotFound("Subscription not found.");

        _context.Subscriptions.Remove(subscription);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Unsubscribed successfully." });
    }

}


