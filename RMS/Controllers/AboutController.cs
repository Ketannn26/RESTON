using Microsoft.AspNetCore.Mvc;

namespace RMS.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
    }
}
