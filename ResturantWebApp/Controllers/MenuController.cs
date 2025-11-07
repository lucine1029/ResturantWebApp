using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantWebApp.Models;
using RestaurantWebApp.Services;
using System.Reflection;

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

        [HttpGet]
        public IActionResult EditDish(int id)  // Display the form to edit an existing dish
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var dish = _restaurantApiService.GetDishByIdAsync(id).Result;
            if (dish == null)
                return NotFound();
            return View(dish);
        }

        [HttpPost]
        public async Task<IActionResult> EditDish(int id, Menu dish)  // Handle form submission to update an existing dish
        {
            if (!ModelState.IsValid)
                return View(dish);
            //// Add debug logging
            //Console.WriteLine($"=== EDIT DISH SUBMISSION ===");
            //Console.WriteLine($"ID: {dish.Id}");
            //Console.WriteLine($"Name: {dish.DishName}");
            //Console.WriteLine($"IsSpicy: {dish.IsSpicy}");
            //Console.WriteLine($"IsVegan: {dish.IsVegan}");
            //Console.WriteLine($"HasNuts: {dish.HasNuts}");
            //Console.WriteLine($"HasEgg: {dish.HasEgg}");
            //Console.WriteLine($"HasDairy: {dish.HasDairy}");
            //Console.WriteLine($"IsGlutenFree: {dish.IsGlutenFree}");
            //Console.WriteLine($"IsAvailable: {dish.IsAvailable}");
            //Console.WriteLine($"Price: {dish.Price}");
            //Console.WriteLine($"=============================");

            var success = await _restaurantApiService.UpdateDishAsync(id, dish);
            if (success)
                return RedirectToAction("AdminMenu");
            else
            {
                ModelState.AddModelError("", "Failed to update dish. Please try again.");
                return View(dish);
            }
        }

        public IActionResult DeleteDish(int id)  // For comfirmation before deletion
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var dish = _restaurantApiService.GetDishByIdAsync(id).Result;
            if (dish == null)
                return NotFound();
            return View(dish);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDishConfirmation(int id)  // Handle deletion of a dish
        {
            try
            {
                var result = await _restaurantApiService.DeleteDishAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Dish deleted successfully!";
                    Console.WriteLine("Successfully deleted.");//for debugging

                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delte dish.";
                    Console.WriteLine("Failed to delte dish.");//for debugging
                }
                return RedirectToAction("AdminMenu");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting dish: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
