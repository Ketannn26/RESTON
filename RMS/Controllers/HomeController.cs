using Microsoft.AspNetCore.Mvc;
using RMS.Models;
using System.Diagnostics;

namespace RMS.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Reston()
        {
            return View();
        }
    }
}
