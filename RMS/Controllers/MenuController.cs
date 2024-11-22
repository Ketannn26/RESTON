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
            var userRole = HttpContext.Session.GetString("UserRole");
            var menus = _context.Menus.ToList();
            ViewBag.UserRole = userRole; // Pass role to the view
            return View(menus);
        }
 
    }
}
