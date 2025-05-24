using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace _3AashYaCoach._3ash_ya_coach.Shared
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }

}
