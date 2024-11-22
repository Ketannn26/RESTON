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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMenu(Menu menu, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream); // Save the image to the server
                    }

                    menu.ImageUrl = "/images/" + fileName; // Store the relative path to the image in the database
                }

                // Save menu item to the database
                _context.Menus.Add(menu);
                await _context.SaveChangesAsync();

                return RedirectToAction("MenuList"); // Redirect to the menu page
            }

            return View(menu); // Return to the AddMenu form if validation fails
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
            if (ModelState.IsValid)
            {
                // Fetch the existing menu item from the database
                var existingMenuItem = await _context.Menus.FindAsync(menu.MenuItemID);

                if (existingMenuItem == null)
                {
                    return NotFound(); // Handle if the menu item does not exist
                }

                // Update the menu properties (excluding the ImageUrl if no new image is uploaded)
                existingMenuItem.ItemName = menu.ItemName;
                existingMenuItem.Price = menu.Price;
                existingMenuItem.IsAvailable = menu.IsAvailable;

                // Handle image upload if a new image is provided
                if (Image != null && Image.Length > 0)
                {
                    // Delete the old image file (if necessary)
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    var oldImagePath = Path.Combine(uploadsFolder, Path.GetFileName(existingMenuItem.ImageUrl.TrimStart('/')));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath); // Delete the old image file
                    }

                    // Save the new image file
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    // Update the ImageUrl property
                    existingMenuItem.ImageUrl = "/images/" + fileName;
                }

                // Save the changes to the database
                _context.Menus.Update(existingMenuItem);
                await _context.SaveChangesAsync();

                return RedirectToAction("MenuList"); // Redirect to the menu list page after updating
            }

            return View(menu); // Return to the EditMenu form if validation fails
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
        public IActionResult DeleteMenu(int id)
        {
            var menu = _context.Menus.Find(id);
            try
            {
                // Your logic to delete the menu item
                _context.Menus.Remove(menu);
                _context.SaveChanges();

                // Add a success message to TempData
                TempData["Message"] = "Menu item deleted successfully!";
            }
            catch (Exception ex)
            {
                // Handle error
                TempData["Message"] = "Error deleting the menu item.";
            }

            return RedirectToAction("MenuList");

        }



    }
}
