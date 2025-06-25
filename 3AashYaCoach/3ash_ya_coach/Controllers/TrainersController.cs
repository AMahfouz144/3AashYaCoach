using _3AashYaCoach._3ash_ya_coach.Services.SubscriptionService;
using _3AashYaCoach._3ash_ya_coach.Services.Trainers;
using _3AashYaCoach.Models.Context;
using _3AashYaCoach.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _3AashYaCoach.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/Trainers")]
    public class TrainersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITrainerService _trainerService;
        private readonly ISubscriptionService _subscriptionService;
        public TrainersController(AppDbContext context, ITrainerService trainerService, ISubscriptionService subscriptionService)
        {
            _context = context;
            _subscriptionService = subscriptionService;
            _trainerService = trainerService;
        }
        [HttpGet("GetTrainers")]
        public async Task<IActionResult> GetAllTrainers()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Invalid token"));
            var result = await _trainerService.GetAllTrainersWithSubscriptionStatus(userId);
            return Ok(result);
        }



        [HttpGet("GetSubscribers/{trainerId}")]
        public async Task<IActionResult> GetSubscribersForTrainer(Guid trainerId)
        {
            var result = await _subscriptionService.GetSubscribersForTrainerAsync(trainerId);
            if (result == null)
                return NotFound("Coach not found or invalid.");

            return Ok(result);
        }

    }

}
