using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _3AashYaCoach.Models.Context;
using _3AashYaCoach.Models.Enums;

namespace _3AashYaCoach.ViewsControllers
{
    //[Authorize(Roles = "Admin")] // لو حابب تمنع الوصول لغير الأدمن
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalMembers = await _context.Users.CountAsync(u => u.Role == UserRole.Trainee);
            ViewBag.ActiveTrainers = await _context.Users.CountAsync(u => u.Role == UserRole.Coach);
            ViewBag.UpcomingSessions = 12; // قيمة تجريبية – بدّلها بما يناسبك
            ViewBag.TotalAnnouncements = 3; // قيمة تجريبية – بدّلها بما يناسبك

            return View();
        }
    }
}
