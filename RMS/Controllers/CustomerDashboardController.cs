using Microsoft.AspNetCore.Mvc;

namespace RMS.Controllers
{
    public class CustomerDashboardController : Controller
    {
        public IActionResult Customer()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "Customer")
            {
                return RedirectToAction("SignIn", "Account");
            }

            // Pass UserFullName to the view
            ViewBag.FullName = HttpContext.Session.GetString("UserFullName");

            return View();
        }
    }
}
