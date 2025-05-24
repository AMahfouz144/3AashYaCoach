
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _3AashYaCoach.Models;
using _3AashYaCoach.Models.Context;
using _3AashYaCoach.ViewModel;
using _3AashYaCoach.Models.Enums;

namespace _3AashYaCoach.ViewsControllers
{
    public class CoachesController : Controller
    {
        private readonly AppDbContext _context;

        public CoachesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Coaches
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Trainers.Include(c => c.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Coaches/Details/5
        //public async Task<IActionResult> Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var coach = await _context.Trainers
        //        .Include(c => c.User)
        //        .FirstOrDefaultAsync(m => m.UserId == id);
        //    if (coach == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(coach);
        //}

        public IActionResult Create()
        {
            return View();
        }

        // POST: Coaches/CreateFull
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCoachViewModel model, IFormFile? ProfileImage)
        {
            if (!ModelState.IsValid)
                return View(model);

            // تحقق من وجود إيميل مكرر
            var exists = await _context.Users.AnyAsync(u => u.Email == model.Email);
            if (exists)
            {
                ModelState.AddModelError("Email", "This email already exists.");
                return View(model);
            }

            // إنشاء مستخدم جديد
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = model.FullName,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = UserRole.Coach
            };
          

            var coach = new Coach
            {
                UserId = user.Id,
                Certificates = model.Certificates,
                ExperienceDetails = model.ExperienceDetails
            };
            
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(ProfileImage.FileName)}";
                var path = Path.Combine("wwwroot/uploads/coaches", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                using var stream = new FileStream(path, FileMode.Create);
                await ProfileImage.CopyToAsync(stream);

                coach.ProfileImagePath = $"/uploads/coaches/{fileName}";
            }
            _context.Users.Add(user);
            _context.Trainers.Add(coach);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

       
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var coach = await _context.Trainers
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == id);

            if (coach == null) return NotFound();

            var model = new EditCoachViewModel
            {
                UserId = coach.UserId,
                FullName = coach.User.FullName,
                Email = coach.User.Email,
                Certificates = coach.Certificates,
                ExperienceDetails = coach.ExperienceDetails
            };

            return View(model);
        }

        // POST: Coaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditCoachViewModel model)
        {
            if (id != model.UserId) return NotFound();

            if (!ModelState.IsValid) return View(model);

            var coach = await _context.Trainers.Include(c => c.User).FirstOrDefaultAsync(c => c.UserId == id);
            if (coach == null) return NotFound();

            // تحديث بيانات المستخدم
            coach.User.FullName = model.FullName;
            coach.User.Email = model.Email;

            // تحديث بيانات المدرب
            coach.Certificates = model.Certificates;
            coach.ExperienceDetails = model.ExperienceDetails;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Coaches/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coach = await _context.Trainers
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (coach == null)
            {
                return NotFound();
            }

            return View(coach);
        }

        // POST: Coaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var coach = await _context.Trainers.FindAsync(id);
            if (coach != null)
            {
                _context.Trainers.Remove(coach);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoachExists(Guid id)
        {
            return _context.Trainers.Any(e => e.UserId == id);
        }
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var coach = await _context.Trainers
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.UserId == id);

            if (coach == null) return NotFound();

            return View(coach);
        }

    }
}
