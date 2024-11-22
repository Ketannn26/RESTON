using Microsoft.AspNetCore.Mvc;
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
            ViewBag.SuccessMessage = "Menu added successfully!";
            return View(menus);
        }

        // GET: AddMenu (Form for adding a menu item)
        public IActionResult AddMenu()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMenu(Menu menu, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                if (Image != null && Image.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    menu.ImageUrl = "/images/" + fileName; // Assign image URL to menu
                }

                _context.Menus.Add(menu); // Add menu to the database
                await _context.SaveChangesAsync(); // Save changes to the database

                TempData["Message"] = "Menu item added successfully!";
                return RedirectToAction("MenuList"); // Redirect to the menu list
            }

            TempData["Message"] = "Failed to add menu item. Please check the details.";
            return View(menu); // Return the view with the current model
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMenu(Menu menu, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                // Fetch existing menu item
                var existingMenu = await _context.Menus.FindAsync(menu.MenuItemID);
                if (existingMenu != null)
                {
                    // Update fields
                    existingMenu.ItemName = menu.ItemName;
                    existingMenu.Price = menu.Price;
                    existingMenu.IsAvailable = menu.IsAvailable;

                    // Handle image upload
                    if (Image != null && Image.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Image.CopyToAsync(stream);
                        }

                        existingMenu.ImageUrl = "/images/" + fileName; // Update image path
                    }

                    _context.Menus.Update(existingMenu);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("MenuList"); // Redirect to menu list
                }
            }

            return View(menu); // Return view if validation fails
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMenu(int id)
        {
            try
            {
                // Find the menu item
                var menu = _context.Menus.Find(id);
                if (menu == null)
                {
                    TempData["Message"] = "Menu item not found!";
                    return RedirectToAction("MenuList");
                }

                // Delete the menu item
                _context.Menus.Remove(menu);
                _context.SaveChanges();

                TempData["Message"] = "Menu item deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "An error occurred while deleting the menu item.";
                Console.WriteLine($"Error: {ex.Message}");
            }

            return RedirectToAction("MenuList");
        }






    }
}
