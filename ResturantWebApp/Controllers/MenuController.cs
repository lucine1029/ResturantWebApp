using Microsoft.AspNetCore.Mvc;

namespace RestaurantWebApp.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
