using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantWebApp.Models;
using RestaurantWebApp.Services;

namespace RestaurantWebApp.Controllers
{
    public class MenuController : Controller
    {
        private readonly IRestaurantApiService _restaurantApiService;
        public MenuController(IRestaurantApiService restaurantApiService)
        {
            _restaurantApiService = restaurantApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Menu";

            var dishList = await _restaurantApiService.GetAllDishesAsync();

            return View(dishList);
        }

        [HttpGet]
        public IActionResult AdminMenu()
        {
            ViewData["Title"] = "Menu Management System";
            try
            {
                var dishList = _restaurantApiService.GetAllDishesAsync().Result;
                return View(dishList ?? new List<Menu>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching menu for admin: {ex.Message}");
                return View(new List<Menu>());
            }
        }

        [HttpGet]
        public IActionResult AddDish()  // Display the form to add a new dish
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDish(DishViewModel newDish)  // Handle form submission to add a new dish
        {
            if (!ModelState.IsValid)
                return View(newDish);
            var success = await _restaurantApiService.AddDishAsync(newDish);
            if (success)
                return RedirectToAction("AdminMenu");
            else
            {
                ModelState.AddModelError("", "Failed to add dish. Please try again.");
                return View(newDish);
            }
        }

        //[HttpGet]
        //public IActionResult EditDish(int id)  // Display the form to edit an existing dish
        //{
        //    if(id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var dish = _restaurantApiService.GetDishByIdAsync(id).Result;
        //    if (dish == null)
        //        return NotFound();
        //    return View(dish);
        //}


    }
}
