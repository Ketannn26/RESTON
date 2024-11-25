using Microsoft.AspNetCore.Mvc;

namespace RMS.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Contact()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            ViewBag.UserRole = userRole;
            return View();
        }
    }
}
