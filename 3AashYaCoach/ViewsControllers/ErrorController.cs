using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.ViewsControllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult Error404()
        {
            return View("NotFound");
        }

        [Route("Error/{statusCode}")]
        public IActionResult HandleError(int statusCode)
        {
            if (statusCode == 404)
                return RedirectToAction("Error404");

            return View("Error");
        }
    }
}