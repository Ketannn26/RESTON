using Microsoft.AspNetCore.Mvc;
using RMS.Models;
using System.Diagnostics;

namespace RMS.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.UserRole = User.IsInRole("Admin") ? "Admin" : User.IsInRole("Customer") ? "Customer" : null;
            return View();
        }
    }
}
