using Microsoft.AspNetCore.Mvc;

namespace RMS.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult About()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            ViewBag.UserRole = userRole;
            return View();
        }
    }
}
