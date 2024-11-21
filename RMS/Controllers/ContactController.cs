using Microsoft.AspNetCore.Mvc;

namespace RMS.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Contact()
        {
            return View();
        }
    }
}
