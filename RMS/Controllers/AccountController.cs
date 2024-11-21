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
                    ModelState.AddModelError("", "Email is already registered.");
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

            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                // Store user information in session (optional)
                //HttpContext.Session.SetString("UserRole", user.Role);
                //HttpContext.Session.SetString("UserFullName", user.FullName);
                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetString("UserFullName", user.FullName);

                if (user.Role == "Admin")
                    return RedirectToAction("Admin", "AdminDashboard");
                else
                    return RedirectToAction("Customer", "CustomerDashboard");
            }
            ViewBag.ErrorMessage = "Invalid credentials";
            return View();
        }

        // Profile
        [HttpGet]
        public IActionResult Profile()
        {
            // Retrieve the user's information from session
            var userFullName = HttpContext.Session.GetString("UserFullName");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userFullName == null || userRole == null)
            {
                // If no session exists, redirect to SignIn
                return RedirectToAction("SignIn");
            }

            // Retrieve full user details from the database (optional, if needed)
            var user = _context.Users.FirstOrDefault(u => u.FullName == userFullName && u.Role == userRole);
            if (user == null)
            {
                return RedirectToAction("SignIn");
            }

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
            //HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserRole");
            HttpContext.Session.Remove("UserFullName");
            return RedirectToAction("SignIn");
        }
    }
}
