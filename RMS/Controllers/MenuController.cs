using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RMS.Data;
using RMS.Models;

namespace RMS.Controllers
{
    public class MenuController : Controller
    {
        private readonly RMSDbContext _context;

        public MenuController(RMSDbContext context)
        {
            _context = context;
        }

        // GET: Menu
        public IActionResult Menu()
        {
            var menus = _context.Menus.ToList(); // Retrieve all menu items from the Menus table

            // Pass the menu items to the view
            return View(menus);
        }
 
    }
}
