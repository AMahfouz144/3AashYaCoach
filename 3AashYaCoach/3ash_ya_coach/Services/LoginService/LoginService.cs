
using _3AashYaCoach.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _3AashYaCoach._3ash_ya_coach.Services.LoginService
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _context.Users
                .Include(u => u.SubscriptionsAsTrainee)
                .Include(u => u.SavedCoaches)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim("Name", user.FullName),
        new Claim(ClaimTypes.Role, user.Role.ToString()),
        new Claim("Email", user.Email)
    };

            // إضافة معرفات الاشتراكات (SubscriptionId) و CoachId
            //foreach (var subscription in user.SubscriptionsAsTrainee)
            //{
            //    claims.Add(new Claim("SubscriptionId", subscription.Id.ToString()));
            //    claims.Add(new Claim("CoachId", subscription.CoachId.ToString()));
            //}

            // إضافة المدرّبين المحفوظين
            //foreach (var saved in user.SavedCoaches)
            //{
            //    claims.Add(new Claim("SavedCoachId", saved.CoachId.ToString()));
            //}

            // إذا كنت تريد تضمين الخطط المشترك فيها أيضًا:
            //var planSubscriptions = await _context.PlanSubscriptions
            //    .Where(ps => ps.TraineeId == user.Id)
            //    .ToListAsync();

            //foreach (var planSub in planSubscriptions)
            //{
            //    claims.Add(new Claim("PlanId", planSub.WorkoutPlanId.ToString()));
            //}

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
