using Microsoft.AspNetCore.Mvc;

namespace RestaurantWebApp.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
