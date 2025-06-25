using _3AashYaCoach._3ash_ya_coach.Models;
using _3AashYaCoach.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace _3AashYaCoach._3ash_ya_coach.Services.DeviceTokenService
{

    public class DeviceTokenService : IDeviceTokenService
    {
        private readonly AppDbContext _context;

        public DeviceTokenService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveOrUpdateTokenAsync(string userId, string token)
        {
            var existing = await _context.DeviceTokens
                .FirstOrDefaultAsync(t => t.UserId == userId);

            if (existing != null)
            {
                existing.Token = token;
                existing.CreatedAt = DateTime.UtcNow;
            }
            else
            {
                var deviceToken = new DeviceToken
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Token = token,
                    CreatedAt = DateTime.UtcNow
                };
                _context.DeviceTokens.Add(deviceToken);
            }

            await _context.SaveChangesAsync();
        }
    }
}
