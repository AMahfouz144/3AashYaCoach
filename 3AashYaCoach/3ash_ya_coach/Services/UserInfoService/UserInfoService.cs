using _3AashYaCoach._3ash_ya_coach.Dtos;
using _3AashYaCoach.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace _3AashYaCoach._3ash_ya_coach.Services.UserInfoService
{
    public class UserInfoService : IUserInfoService
    {
        private readonly AppDbContext _context;

        public UserInfoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserInfoDto> GetUserInfoAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.SubscriptionsAsTrainee)
                .Include(u => u.SavedCoaches)
                .Include(u => u.SubscriptionsAsTrainee)
                    .ThenInclude(s => s.PlanSubscriptions)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) throw new Exception("User not found");

            var dto = new UserInfoDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                SubscriptionIds = user.SubscriptionsAsTrainee?.Select(s => s.Id).ToList() ?? [],
                CoachIds = user.SubscriptionsAsTrainee?.Select(s => s.CoachId).ToList() ?? [],
                SavedCoachIds = user.SavedCoaches?.Select(sc => sc.CoachId).ToList() ?? [],
                PlanIds = user.SubscriptionsAsTrainee?
                    .SelectMany(s => s.PlanSubscriptions.Select(ps => ps.WorkoutPlanId))
                    .Distinct()
                    .ToList() ?? []
            };

            return dto;
        }
    }

}
