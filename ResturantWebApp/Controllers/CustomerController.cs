using Microsoft.AspNetCore.Mvc;

namespace RestaurantWebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly HttpClient _httpClient;
        private string _baseUrl = "https://localhost:7180"; //call the API

        public CustomerController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
