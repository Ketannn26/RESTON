using Microsoft.AspNetCore.Mvc;
using RMS.Data;

namespace RMS.Controllers
{
    public class BookTableController : Controller
    {
        private readonly RMSDbContext _context; // Replace with your DB context class

        public BookTableController(RMSDbContext context)
        {
            _context = context;
        }

        // GET: BookTable
        // Booking Form
        // Landing Page
        public IActionResult BookTableLanding()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            ViewBag.UserRole = userRole; // Pass user role to the view
            return View();
        }

        // Booking Form
        public IActionResult BookTable()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userRole))
            {
                TempData["ErrorMessage"] = "Please sign in to book a table.";
                return RedirectToAction("SignIn", "Account");
            }

            return View(); // Display the booking form if logged in
        }

    }
}
