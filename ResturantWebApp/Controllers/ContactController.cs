using Microsoft.AspNetCore.Mvc;

namespace RestaurantWebApp.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
