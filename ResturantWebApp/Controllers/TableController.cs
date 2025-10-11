using Microsoft.AspNetCore.Mvc;

namespace RestaurantWebApp.Controllers
{
    public class TableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
