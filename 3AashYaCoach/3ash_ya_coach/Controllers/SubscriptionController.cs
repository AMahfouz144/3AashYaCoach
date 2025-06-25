using _3AashYaCoach.Models.Context;
using _3AashYaCoach.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using _3AashYaCoach.Dtos;
using _3AashYaCoach.Models.Enums;
using Microsoft.AspNetCore.SignalR;
using _3AashYaCoach._3ash_ya_coach.Services.NotificationService;
using _3AashYaCoach._3ash_ya_coach.Services.NotificationService.Firebase;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly IFirebaseNotificationService _firebaseNotificationService;

    public SubscriptionsController( AppDbContext context, IHubContext<NotificationHub> hubContext,IFirebaseNotificationService firebaseNotificationService)
    {
        _context = context;
        _hubContext = hubContext;
        _firebaseNotificationService = firebaseNotificationService;
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
        // SignalR notification
        await _hubContext.Clients
            .User(dto.CoachId.ToString())
            .SendAsync("ReceiveNotification", $"A new trainee subscribed to you: {trainee.FullName}");

        // Firebase Push Notification
        var deviceToken = await _context.DeviceTokens
            .Where(d => d.UserId == dto.CoachId.ToString())
            .Select(d => d.Token)
            .FirstOrDefaultAsync();

        if (!string.IsNullOrEmpty(deviceToken))
        {
            await _firebaseNotificationService.SendNotificationAsync(
                deviceToken,
                "New Subscription",
                $"{trainee.FullName} has subscribed to your training.");
        }
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


