using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RMS.Data;
using RMS.Models;


namespace RMS.Controllers
{
    [Route("[controller]/[action]")]
    public class AdminDashboardController : Controller
    {
        private readonly RMSDbContext _context;

        public AdminDashboardController(RMSDbContext context)
        {
            _context = context;
        }
        public IActionResult Admin()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            // If no user is logged in, redirect to home page
            if (string.IsNullOrEmpty(userRole))
            {
                return RedirectToAction("Reston", "Home");  // Redirect to the home page if not logged in
            }

            if (userRole != "Admin")
            {
                return RedirectToAction("SignIn", "Account");
            }

            ViewBag.UserFullName = HttpContext.Session.GetString("UserFullName");
            ViewBag.UserRole = userRole;

            return View();
        }

        // GET: Menu
        public IActionResult MenuList()
        {
            
            var menus = _context.Menus.ToList();
            ViewBag.Message = TempData["Message"]?.ToString();
            return View(menus);
        }

        // GET: Create Menu Item
        public IActionResult CreateMenuItem()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Unauthorized(); // Prevent unauthorized access
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateMenuItem(Menu menu, IFormFile ImageUrl)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Unauthorized(); // Prevent unauthorized access
            }

            if (!ModelState.IsValid)
            {
                if (ImageUrl != null && ImageUrl.Length > 0)
                {
                    // Save the uploaded image file
                    var fileName = Path.GetFileName(ImageUrl.FileName);
                    var filePath = Path.Combine("wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageUrl.CopyTo(stream);
                    }

                    // Update the menu's ImageUrl with the file path
                    menu.ImageUrl = "/images/" + fileName;
                }

                // Save the menu item to the database
                _context.Menus.Add(menu);
                _context.SaveChanges();

                TempData["Message"] = "Menu item added successfully!";
                return RedirectToAction(nameof(MenuList));
            }

            return View(menu);
        }


        // GET: Delete Menu Item (Confirmation Page)
        public IActionResult DeleteMenuItem(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Unauthorized(); // Prevent unauthorized access
            }

            var menu = _context.Menus.Find(id);
            if (menu == null)
            {
                TempData["Message"] = "Menu item not found.";
                return RedirectToAction(nameof(MenuList));
            }
            return View(menu);
        }

        // POST: Delete Menu Item
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDeleteMenuItem(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Unauthorized(); // Prevent unauthorized access
            }

            var menu = _context.Menus.Find(id);
            if (menu == null)
            {
                TempData["Message"] = "Menu item not found.";
                return RedirectToAction(nameof(MenuList));
            }

            _context.Menus.Remove(menu);
            _context.SaveChanges();
            TempData["Message"] = "Menu item deleted successfully!";
            return RedirectToAction(nameof(MenuList));
        }

        // GET: Edit Menu Item
        public IActionResult EditMenuItem(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Unauthorized(); // Prevent unauthorized access
            }

            var menu = _context.Menus.FirstOrDefault(m => m.MenuItemID == id);
            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMenuItem(Menu menu, IFormFile ImageUpload)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Unauthorized(); // Prevent unauthorized access
            }

            if (!ModelState.IsValid)
            {
                var existingMenu = _context.Menus.FirstOrDefault(m => m.MenuItemID == menu.MenuItemID);
                if (existingMenu == null)
                {
                    return NotFound();
                }

                // Update fields
                existingMenu.ItemName = menu.ItemName;
                existingMenu.Price = menu.Price;
                existingMenu.IsAvailable = menu.IsAvailable;

                if (ImageUpload != null && ImageUpload.Length > 0)
                {
                    // Save the uploaded image file
                    var fileName = Path.GetFileName(ImageUpload.FileName);
                    var filePath = Path.Combine("wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageUpload.CopyTo(stream);
                    }

                    // Update ImageUrl
                    existingMenu.ImageUrl = "/images/" + fileName;
                }

                // Save changes to the database
                _context.Menus.Update(existingMenu);
                _context.SaveChanges();

                TempData["Message"] = "Menu item updated successfully!";
                return RedirectToAction(nameof(MenuList));
            }

            return View(menu);
        }

        // View all users
        public IActionResult UserList()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        // Add a new user
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("UserList");
            }
            return View(user);
        }

        // Edit user (optional)
        public IActionResult EditUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Roles = new SelectList(new List<string> { "Customer", "Admin" }, user.Role);
            return View(user);
        }

        // Edit user (POST)
        [HttpPost]
        public IActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.Find(user.U_Id);
                if (existingUser != null)
                {
                    existingUser.FullName = user.FullName;
                    existingUser.Email = user.Email;
                    existingUser.Role = user.Role;

                    // Only update password if it's changed
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        existingUser.Password = user.Password;
                    }

                    _context.SaveChanges();
                    return RedirectToAction("UserList");
                }
            }
            return View(user);
        }

        // GET: DeleteUser (Confirmation Page)
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.U_Id == id);
            if (user == null)
            {
                return NotFound(); // Handle case where user doesn't exist
            }
            return View(user); // Pass the user to the confirmation view
        }


        [HttpPost]
        public IActionResult ConfirmDeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.U_Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("UserList");
        }


    }
}
