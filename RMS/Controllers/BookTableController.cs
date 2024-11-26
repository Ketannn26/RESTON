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
            // Retrieve the user's role and email from session to check if they're logged in
            var userRole = HttpContext.Session.GetString("UserRole");
            var userEmail = HttpContext.Session.GetString("UserEmail");

            // Check if the user is logged in (both role and email should be set)
            if (string.IsNullOrEmpty(userRole) || string.IsNullOrEmpty(userEmail))
            {
                TempData["ErrorMessage"] = "Please sign in to book a table.";
                return RedirectToAction("SignIn", "Account");
            }

            // Fetch user details from the database using the email stored in session
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found. Please sign in again.";
                return RedirectToAction("SignIn", "Account");
            }

            // Create a new Booking model to pass data to the view
            var booking = new Booking
            {
                UserName = user.FullName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                ReservationDate = DateTime.Now, // Default to the current date and time
                U_Id = user.U_Id
            };

            // Pass the booking model to the view
            return View(booking);
        }

        // POST: BookTable Form
        [HttpPost]
        public IActionResult BookTable(Booking booking)
        {
            // Retrieve the user's role and email from the session to check if they're logged in
            var userRole = HttpContext.Session.GetString("UserRole");
            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userRole) || string.IsNullOrEmpty(userEmail))
            {
                TempData["ErrorMessage"] = "Please sign in to book a table.";
                return RedirectToAction("SignIn", "Account");
            }

            // Fetch user details from the database using the email stored in session
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found. Please sign in again.";
                return RedirectToAction("SignIn", "Account");
            }

            // Set the user information to the booking model
            booking.UserName = user.FullName;
            booking.Email = user.Email;
            booking.Phone = user.PhoneNumber;
            booking.U_Id = user.U_Id;

            // If the model is valid, save the booking
            if (ModelState.IsValid)
            {
                _context.Bookings.Add(booking);
                _context.SaveChanges();

                // Set success message and redirect
                TempData["SuccessMessage"] = "Your table has been successfully booked!";
                return RedirectToAction("BookTableLanding");
            }

            // If the form is not valid, return the view with the booking details
            return View(booking);
        }
    }
}
