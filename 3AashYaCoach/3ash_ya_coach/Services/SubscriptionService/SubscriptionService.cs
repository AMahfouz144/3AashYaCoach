using _3AashYaCoach._3ash_ya_coach.Dtos;
using _3AashYaCoach.Models.Context;
using _3AashYaCoach.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace _3AashYaCoach._3ash_ya_coach.Services.SubscriptionService
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly AppDbContext _context;

        public SubscriptionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SubscribersResultDto?> GetSubscribersForTrainerAsync(Guid trainerId)
        {
            var coach = await _context.Users.FindAsync(trainerId);
            if (coach == null || coach.Role != UserRole.Coach)
                return null;

            var subscribers = await _context.Subscriptions
                .Where(s => s.CoachId == trainerId)
                .Include(s => s.Trainee)
                .Select(s => new SubscriberDto
                {
                    TraineeId = s.TraineeId,
                    FullName = s.Trainee.FullName,
                    Email = s.Trainee.Email,
                    SubscribedAt = s.SubscribedAt,
                    Notes = s.Notes
                })
                .ToListAsync();

            var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);
            var recentCount = subscribers.Count(s => s.SubscribedAt >= oneMonthAgo);

            return new SubscribersResultDto
            {
                TrainerId = trainerId,
                TrainerName = coach.FullName,
                SubscribersCount = subscribers.Count,
                RecentSubscribersCount = recentCount,
                Subscribers = subscribers
            };
        }
    }

}
