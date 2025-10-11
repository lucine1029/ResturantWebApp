using Microsoft.AspNetCore.Mvc;

namespace RestaurantWebApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
