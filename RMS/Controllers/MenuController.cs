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
            return View();
        }
 
    }
}
