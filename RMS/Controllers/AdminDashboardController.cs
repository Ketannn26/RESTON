using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMS.Data;
using RMS.Models;


namespace RMS.Controllers
{
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

            if (userRole != "Admin")
            {
                return RedirectToAction("SignIn", "Account");
            }

            // Pass UserFullName to the view
            ViewBag.FullName = HttpContext.Session.GetString("UserFullName");

            return View();
        }

        // GET: Menu
        public IActionResult MenuList()
        {
            // Fetch all menu items from the database
            var menus = _context.Menus.ToList();
            return View(menus);
        }

        // GET: AddMenu (Form for adding a menu item)
        public IActionResult AddMenu()
        {
            return View();
        }

        // POST: AddMenu (Handles form submission)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMenu(Menu menu)
        {

            if (ModelState.IsValid)
            {
                // Add menu item to the database
                _context.Menus.Add(menu);
                _context.SaveChanges();
                return RedirectToAction("MenuList"); // Redirect to menu list
            }

            // If model is invalid, return to form with validation errors
            return View(menu);


        }

        // GET: EditMenu (Load the menu item for editing)
        public IActionResult EditMenu(int id)
        {
            // Find the menu item by ID
            var menu = _context.Menus.FirstOrDefault(m => m.MenuItemID == id);

            if (menu == null)
            {
                // Return a 404 error if the menu item is not found
                return NotFound();
            }

            // Pass the menu item to the view
            return View(menu);
        }

        // POST: EditMenu (Save changes to the menu item)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMenu(Menu menu)
        {
            if (ModelState.IsValid)
            {
                // Update the menu item in the database
                _context.Menus.Update(menu);
                _context.SaveChanges();

                // Redirect to the menu list after editing
                return RedirectToAction("MenuList");
            }

            // Return to the form if validation fails
            return View(menu);
        }

        // GET: DeleteMenu (Confirm deletion)
        public IActionResult DeleteItem(int id)
        {
            // Find the menu item by ID
            var menu = _context.Menus.FirstOrDefault(m => m.MenuItemID == id);

            if (menu == null)
            {
                // Return a 404 error if the menu item is not found
                return NotFound();
            }

            return View(menu); // Load the confirmation view
        }

        // POST: DeleteMenu (Perform deletion)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDeleteItem(int id)
        {
            // Find the menu item by ID
            var menu = _context.Menus.FirstOrDefault(m => m.MenuItemID == id);

            if (menu == null)
            {
                // Return a 404 error if the menu item is not found
                return NotFound();
            }

            // Remove the menu item from the database
            _context.Menus.Remove(menu);
            _context.SaveChanges();


            // Redirect to the menu list after deletion
            return RedirectToAction("MenuList");
        }


    }
}
