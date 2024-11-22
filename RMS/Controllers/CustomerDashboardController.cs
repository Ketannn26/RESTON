using Microsoft.AspNetCore.Mvc;

namespace RMS.Controllers
{
    public class CustomerDashboardController : Controller
    {
        public IActionResult Customer()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            // If no user is logged in, redirect to home page
            if (string.IsNullOrEmpty(userRole))
            {
                return RedirectToAction("Reston", "Home");  // Redirect to the home page if not logged in
            }

            if (userRole != "Customer")
            {
                return RedirectToAction("SignIn", "Account");
            }

            ViewBag.UserFullName = HttpContext.Session.GetString("UserFullName");
            ViewBag.UserRole = userRole;

            return View();
        }
    }
}
