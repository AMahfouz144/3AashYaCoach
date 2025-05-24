using _3AashYaCoach.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers.ViewsControllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class IdentityController : Controller
    {
        public IActionResult AccessDenied() => View();

        public IActionResult Signin() => View();

        [HttpPost]
        public async Task<IActionResult> Signin(LoginViewModel model,string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            //try
            //{
            //    var claims = await command.Excute(model.Email, model.Password);

            //    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //    var authProperties = new AuthenticationProperties
            //    {
            //        IsPersistent = true,
            //        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            //    };

            //    await HttpContext.SignInAsync(
            //        CookieAuthenticationDefaults.AuthenticationScheme,
            //        new ClaimsPrincipal(claimsIdentity),
            //        authProperties
            //    );


            //    return RedirectToAction("Index", "Home");
            //}
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError("", ex.Message);
            //}

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Signin", "Identity");
        }
    }
}