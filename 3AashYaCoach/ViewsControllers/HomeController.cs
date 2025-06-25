using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _3AashYaCoach.Models.Context;
using _3AashYaCoach.Models.Enums;
using System.Security.Claims;

namespace _3AashYaCoach.ViewsControllers
{
    [Authorize] // يتطلب تسجيل الدخول للوصول للـ Dashboard
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // إضافة debugging
            Console.WriteLine($"Home/Index accessed. User authenticated: {User.Identity.IsAuthenticated}");
            
            // الحصول على معلومات المستخدم الحالي
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            Console.WriteLine($"User ID: {userId}, Role: {userRole}, Name: {userName}");

            ViewBag.UserName = userName;
            ViewBag.UserRole = userRole;

            // إحصائيات عامة
            ViewBag.TotalMembers = await _context.Users.CountAsync(u => u.Role == UserRole.Trainee);
            ViewBag.ActiveTrainers = await _context.Users.CountAsync(u => u.Role == UserRole.Coach);
            ViewBag.TotalWorkoutPlans = await _context.WorkoutPlans.CountAsync();
            ViewBag.TotalSubscriptions = await _context.Subscriptions.CountAsync();

            // إحصائيات خاصة بالمستخدم
            if (Guid.TryParse(userId, out var currentUserId))
            {
                if (userRole == UserRole.Coach.ToString())
                {
                    // إحصائيات المدرب
                    ViewBag.MySubscribers = await _context.Subscriptions.CountAsync(s => s.CoachId == currentUserId);
                    ViewBag.MyWorkoutPlans = await _context.WorkoutPlans.CountAsync(w => w.CoachId == currentUserId);
                    ViewBag.MyActivePlans = await _context.WorkoutPlans.CountAsync(w => w.CoachId == currentUserId && w.IsPublic);
                }
                else if (userRole == UserRole.Trainee.ToString())
                {
                    // إحصائيات المتدرب
                    ViewBag.MySubscriptions = await _context.Subscriptions.CountAsync(s => s.TraineeId == currentUserId);
                    ViewBag.MySavedCoaches = await _context.SavedCoaches.CountAsync(s => s.TraineeId == currentUserId);
                    ViewBag.MyPlanSubscriptions = await _context.PlanSubscriptions.CountAsync(ps => ps.Subscription.TraineeId == currentUserId);
                }
            }

            // إحصائيات إضافية
            ViewBag.UpcomingSessions = 12; // قيمة تجريبية
            ViewBag.TotalAnnouncements = 3; // قيمة تجريبية

            return View();
        }

        // إجراء مؤقت لاختبار تسجيل الدخول بدون [Authorize]
        [AllowAnonymous]
        public IActionResult Test()
        {
            ViewBag.Message = "Test page - no authorization required";
            return Content("Test page works! User is authenticated: " + User.Identity.IsAuthenticated, "text/html");
        }
    }
}
