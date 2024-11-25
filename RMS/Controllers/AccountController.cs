using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMS.Data;
using RMS.Models;

namespace RMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly RMSDbContext _context;

        public AccountController(RMSDbContext context)
        {
            _context = context;
        }

        // Sign Up
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    // Add a custom error message for the email field
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(user);
                }
                // Check if the phone number is exactly 10 digits
                if (user.PhoneNumber.Length != 10)
                {
                    ModelState.AddModelError("PhoneNumber", "Phone number must be exactly 10 digits.");
                    return View(user);
                }

                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("SignIn");
            }
            return View(user);
        }

        // Sign In
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(string email, string password)
        {
            if (!ModelState.IsValid)
            {
                return View(User);
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {

                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetString("UserFullName", user.FullName);

                if (user.Role == "Admin")
                    return RedirectToAction("Admin", "AdminDashboard");
                else if(user.Role == "Customer")
                    return RedirectToAction("Customer", "CustomerDashboard");
            }
            ViewBag.ErrorMessage = "Invalid email or password.";
            return View(User);
        }

        // Profile
        [HttpGet]
        public IActionResult Profile()
        {
            // Retrieve the user's information from session
            var userFullName = HttpContext.Session.GetString("UserFullName");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userFullName) || string.IsNullOrEmpty(userRole))
            {
                // If no session exists, redirect to SignIn
                return RedirectToAction("SignIn", "Account");
            }

            // Retrieve full user details from the database (optional, if needed)
            var user = _context.Users.FirstOrDefault(u => u.FullName == userFullName && u.Role == userRole);
            if (user == null)
            {
                return RedirectToAction("SignIn");
            }

            ViewBag.UserRole = userRole;
            ViewBag.UserFullName = userFullName;
            return View(user); // Pass the user object to the view
        }


        [HttpPost]
        public IActionResult UpdateProfile(User updatedUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == updatedUser.Email);

            if (user != null)
            {
                user.FullName = updatedUser.FullName;

                if (!string.IsNullOrEmpty(updatedUser.Password))
                {
                    user.Password = updatedUser.Password;
                }

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Profile updated successfully!";
            }

            return RedirectToAction("Profile");
        }



        // Sign Out
        public IActionResult SignOut()
        {
            HttpContext.Session.Clear(); // Clear all sessions
            return RedirectToAction("Index","Home");
        }
    }
}
