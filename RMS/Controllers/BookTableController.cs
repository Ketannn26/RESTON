using Microsoft.AspNetCore.Mvc;
using RMS.Data;
using RMS.Models;

namespace RMS.Controllers
{
    public class BookTableController : Controller
    {
        private readonly RMSDbContext _context;

        public BookTableController(RMSDbContext context)
        {
            _context = context;
        }

        // GET: BookTable Landing Page
        public IActionResult BookTableLanding()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            ViewBag.UserRole = userRole; // Pass user role to the view
            return View();
        }

        // GET: BookTable Form
        public IActionResult BookTable()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userRole) || string.IsNullOrEmpty(userEmail))
            {
                TempData["ErrorMessage"] = "Please sign in to book a table.";
                return RedirectToAction("SignIn", "Account");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found. Please sign in again.";
                return RedirectToAction("SignIn", "Account");
            }

            var booking = new Booking
            {
                UserName = user.FullName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                ReservationDate = DateTime.Today.Add(DateTime.Now.TimeOfDay),
                U_Id = user.U_Id
            };

            return View(booking);
        }

        // POST: BookTable Form
        [HttpPost]
        public IActionResult BookTable(Booking booking)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userRole) || string.IsNullOrEmpty(userEmail))
            {
                TempData["ErrorMessage"] = "Please sign in to book a table.";
                return RedirectToAction("SignIn", "Account");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found. Please sign in again.";
                return RedirectToAction("SignIn", "Account");
            }

            booking.UserName = user.FullName;
            booking.Email = user.Email;
            booking.Phone = user.PhoneNumber;
            booking.U_Id = user.U_Id;

            if (booking.NumberOfGuests <= 0)
            {
                ModelState.AddModelError("NumberOfGuests", "Please specify a valid number of guests.");
            }

            if (!ModelState.IsValid)
            {
                _context.Bookings.Add(booking);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Your table has been successfully booked!";
                return RedirectToAction("BookTableLanding");
            }

            return View(booking);
        }
    }
}
