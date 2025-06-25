using _3AashYaCoach.Models.Context;
using _3AashYaCoach.ViewsControllers.Models;
using _3AashYaCoach._3ash_ya_coach.Services.LoginService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
#region
//namespace API.Controllers.ViewsControllers
//{
//    [ApiExplorerSettings(IgnoreApi = true)]
//    public class IdentityController : Controller
//    {
//        public IActionResult AccessDenied() => View();

//        public IActionResult Signin() => View();

//        [HttpPost]
//        public async Task<IActionResult> Signin(LoginViewModel model, string returnUrl)
//        {
//            if (!ModelState.IsValid)
//                return View(model);

//            try
//            {
//                var claims = await Command.Excute(model.Email, model.Password);

//                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//                var authProperties = new AuthenticationProperties
//                {
//                    IsPersistent = true,
//                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
//                };

//                await HttpContext.SignInAsync(
//                    CookieAuthenticationDefaults.AuthenticationScheme,
//                    new ClaimsPrincipal(claimsIdentity),
//                    authProperties
//                );


//                return RedirectToAction("Index", "Home");
//            }
//            catch (Exception ex)
//            {
//                ModelState.AddModelError("", ex.Message);
//            }

//            return View(model);
//        }

//        public async Task<IActionResult> Logout()
//        {
//            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//            return RedirectToAction("Signin", "Identity");
//        }
//    }
//}
#endregion
namespace _3AashYaCoach.ViewsControllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class IdentityController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly AppDbContext _context;

        public IdentityController(ILoginService loginService, AppDbContext context)
        {
            _loginService = loginService;
            _context = context;
        }

        public IActionResult AccessDenied() => View();

        public IActionResult Signin() => View();

        [HttpPost]
        public async Task<IActionResult> Signin(LoginViewModel model, string returnUrl = null)
        {
            //Console.WriteLine($"=== LOGIN ATTEMPT ===");
            //Console.WriteLine($"Email: {model?.Email}");
            //Console.WriteLine($"Password: {(string.IsNullOrEmpty(model?.Password) ? "NULL" : "PROVIDED")}");
            //Console.WriteLine($"RememberMe: {model?.RememberMe}");
            //Console.WriteLine($"ReturnUrl: {returnUrl}");
            //Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
            
            if (!ModelState.IsValid)
            {
                //Console.WriteLine("=== MODEL STATE ERRORS ===");
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    if (errors.Any())
                    {
                        Console.WriteLine($"Key: {key}");
                        foreach (var error in errors)
                        {
                            Console.WriteLine($"  Error: {error.ErrorMessage}");
                        }
                    }
                }
                return View(model);
            }

            try
            {
                // التحقق من صحة بيانات تسجيل الدخول
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                Console.WriteLine($"User found: {user != null}");

                if (user == null)
                {
                    Console.WriteLine("User not found");
                    ModelState.AddModelError("", "Invalid email or password");
                    return View(model);
                }

                var passwordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);
                Console.WriteLine($"Password valid: {passwordValid}");

                if (!passwordValid)
                {
                    Console.WriteLine("Invalid password");
                    ModelState.AddModelError("", "Invalid email or password");
                    return View(model);
                }

                Console.WriteLine($"User authenticated successfully: {user.Email}, Role: {user.Role}");

                // إنشاء Claims للمستخدم
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(1)
                };

                Console.WriteLine("Signing in user...");
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                Console.WriteLine("User signed in successfully");

                // إعادة التوجيه إلى الصفحة المطلوبة أو الصفحة الرئيسية
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    Console.WriteLine($"Redirecting to returnUrl: {returnUrl}");
                    return Redirect(returnUrl);
                }

                // التوجيه إلى Dashboard بعد تسجيل الدخول
                Console.WriteLine("Redirecting to Home/Index");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", "An error occurred during login. Please try again.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Signin", "Identity");
        }

        // إجراء مؤقت لإنشاء مستخدم تجريبي
        public async Task<IActionResult> CreateTestUser()
        {
            try
            {
                Console.WriteLine("Creating test user...");
                
                // التحقق من وجود المستخدم
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
                
                if (existingUser == null)
                {
                    var password = "123456";
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                    
                    Console.WriteLine($"Creating new user with email: test@example.com");
                    Console.WriteLine($"Hashed password: {hashedPassword}");
                    
                    var testUser = new _3AashYaCoach.Models.User
                    {
                        Email = "test@example.com",
                        PasswordHash = hashedPassword,
                        FullName = "Test User",
                        Role = _3AashYaCoach.Models.Enums.UserRole.Trainee
                    };

                    _context.Users.Add(testUser);
                    await _context.SaveChangesAsync();
                    
                    Console.WriteLine("Test user created successfully");
                    
                    return Content("Test user created successfully!<br/>Email: test@example.com<br/>Password: 123456<br/><a href='/Identity/Signin'>Go to Login Page</a>", "text/html");
                }
                else
                {
                    Console.WriteLine("Test user already exists");
                    
                    // تحديث كلمة المرور للتأكد من أنها صحيحة
                    var password = "123456";
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                    existingUser.PasswordHash = hashedPassword;
                    await _context.SaveChangesAsync();
                    
                    Console.WriteLine("Test user password updated");
                    
                    return Content("Test user already exists! Password updated.<br/>Email: test@example.com<br/>Password: 123456<br/><a href='/Identity/Signin'>Go to Login Page</a>", "text/html");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating test user: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return Content($"An error occurred: {ex.Message}", "text/html");
            }
        }

        // إجراء مؤقت لاختبار قاعدة البيانات
        public async Task<IActionResult> TestDatabase()
        {
            try
            {
                Console.WriteLine("Testing database connection...");
                
                var totalUsers = await _context.Users.CountAsync();
                Console.WriteLine($"Total users in database: {totalUsers}");
                
                var testUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
                if (testUser != null)
                {
                    Console.WriteLine($"Test user found: {testUser.Email}, Role: {testUser.Role}");
                    return Content($"Database connection successful!<br/>Total users: {totalUsers}<br/>Test user: {testUser.Email}<br/>Role: {testUser.Role}", "text/html");
                }
                else
                {
                    Console.WriteLine("Test user not found");
                    return Content($"Database connection successful!<br/>Total users: {totalUsers}<br/>Test user not found", "text/html");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return Content($"Database error: {ex.Message}", "text/html");
            }
        }
    }
}