using _3AashYaCoach._3ash_ya_coach.Dtos;
using _3AashYaCoach._3ash_ya_coach.Models;
using _3AashYaCoach.Models.Context;
using _3AashYaCoach.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace _3AashYaCoach._3ash_ya_coach.Services.PlanSubscriptionService
{
    public class PlanSubscriptionService:IPlanSubscriptionService
    {
        private readonly AppDbContext _context;

        public PlanSubscriptionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string?> SubscribeAsync(SubscribeToPlanDto dto)
        {
            var trainee = await _context.Users.FindAsync(dto.TraineeId);
            if (trainee == null || trainee.Role != UserRole.Trainee)
                return "Invalid trainee.";

            var plan = await _context.WorkoutPlans
                .FirstOrDefaultAsync(p => p.Id == dto.WorkoutPlanId);
            if (plan == null)
                return "Workout plan not found.";

            var coachId = plan.CoachId;

            var subscription = await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.TraineeId == dto.TraineeId && s.CoachId == coachId);
            if (subscription == null)
                return "You must be subscribed to this coach before subscribing to the plan.";

            var exists = await _context.PlanSubscriptions
                .AnyAsync(x => x.TraineeId == dto.TraineeId && x.WorkoutPlanId == dto.WorkoutPlanId);
            if (exists)
                return "Already subscribed to this plan.";

            var sub = new PlanSubscription
            {
                Id = Guid.NewGuid(),
                WorkoutPlanId = dto.WorkoutPlanId,
                TraineeId = dto.TraineeId,
                SubscriptionId = subscription.Id
            };

            _context.PlanSubscriptions.Add(sub);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<string?> UnsubscribeAsync(SubscribeToPlanDto dto)
        {
            var subscription = await _context.PlanSubscriptions
                .FirstOrDefaultAsync(x => x.TraineeId == dto.TraineeId && x.WorkoutPlanId == dto.WorkoutPlanId);

            if (subscription == null)
                return "Subscription not found.";

            _context.PlanSubscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<int> GetSubscribersCountAsync(Guid planId)
        {
            return await _context.PlanSubscriptions.CountAsync(x => x.WorkoutPlanId == planId);
        }
    }
}
