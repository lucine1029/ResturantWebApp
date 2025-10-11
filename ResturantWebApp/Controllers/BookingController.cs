using Microsoft.AspNetCore.Mvc;

namespace RestaurantWebApp.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
