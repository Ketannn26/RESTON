using Microsoft.AspNetCore.Mvc;

namespace RMS.Controllers
{
    public class BookTableController : Controller
    {
        public IActionResult BookTable()
        {
            return View();
        }
    }
}
