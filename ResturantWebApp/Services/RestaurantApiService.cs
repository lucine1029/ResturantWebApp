using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestaurantWebApp.Models;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace RestaurantWebApp.Services
{
    public class RestaurantApiService : IRestaurantApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RestaurantApiService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<List<Menu>> GetAllDishesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/getalldishes");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var dishList = System.Text.Json.JsonSerializer.Deserialize<List<Menu>>(json, new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return dishList;
                }
                else
                {
                    return new List<Menu>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching menu: {ex.Message}");
                return new List<Menu>();
            }

        }

        public async Task<string> LoginAsync(LoginViewModel loginViewModel)
        {
            var json = JsonConvert.SerializeObject(loginViewModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/login", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Login successful."); //for debugging
            Console.WriteLine($"Login response: {result}");  //for debugging
            dynamic data = JsonConvert.DeserializeObject(result);

            string token = data?.token;
            return token;

        }

        public async Task<bool> LogoutAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync($"{_baseUrl}/logout", null);
            return response.IsSuccessStatusCode;

        }

        public async Task<bool> AddDishAsync(DishViewModel newDish)
        {
            // Get token from cookie
            var token = _httpContextAccessor.HttpContext.Request.Cookies["JWToken"];

            // Add it to Authorization header
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var json = JsonConvert.SerializeObject(newDish);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/createmenu", content);


            Console.WriteLine($"CreateDishAsync: Response status = {response.StatusCode}"); // for debugging

            return response.IsSuccessStatusCode;
        }

        public async Task<Menu> GetDishByIdAsync(int id)
        {
            // Get token from cookie
            var token = _httpContextAccessor.HttpContext.Request.Cookies["JWToken"];

            // Add it to Authorization header
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/getdishbyid/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Menu>(content);
                }
                else
                {
                    Console.WriteLine($"Failed to get dish {id}. Status: {response.StatusCode}");
                    return null;
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP error getting dish {id}: {httpEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting dish {id}: {ex.Message}");
                return null;
            }

        }

        public async Task<bool> UpdateDishAsync(int id, Menu dish)
        {
            try
            {
                // Validate that the IDs match
                if (id != dish.Id)
                {
                    Console.WriteLine($"ID mismatch: URL id={id}, Dish id={dish.Id}");
                    return false;
                }
                // Get token from cookie
                var token = _httpContextAccessor.HttpContext.Request.Cookies["JWToken"];

                // Add it to Authorization header
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }

                // Create a clean object to avoid any serialization issues
                var updateData = new
                {
                    Id = dish.Id,
                    DishName = dish.DishName ?? string.Empty, // Ensure not null to match API expectations
                    Description = dish.Description ?? string.Empty,// Ensure not null to match API expectations
                    ImageUrl = dish.ImageUrl ?? string.Empty,// Ensure not null to match API expectations
                    Price = dish.Price,
                    IsAvailable = dish.IsAvailable,
                    IsVegan = dish.IsVegan,
                    HasNuts = dish.HasNuts,
                    HasEgg = dish.HasEgg,
                    HasDairy = dish.HasDairy,
                    IsSpicy = dish.IsSpicy,
                    IsGlutenFree = dish.IsGlutenFree
                };

                // Serialize the updated dish to JSON

                var json = JsonConvert.SerializeObject(dish, Formatting.Indented);
                //Console.WriteLine("=== ACTUAL JSON BEING SENT ===");
                //Console.WriteLine(json);
                //Console.WriteLine("===============================");

                // Send PUT request to update the dish
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/updatemenu/{id}", updateData);

                Console.WriteLine($"UpdateDishAsync: Response status = {response.StatusCode}");

                // Read response content for debugging
                if (!response.IsSuccessStatusCode)
                {
                    //var errorContent = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine($"=== ERROR RESPONSE ===");
                    //Console.WriteLine($"Status: {response.StatusCode}");
                    //Console.WriteLine($"Error: {errorContent}");
                    //Console.WriteLine($"======================");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateDishAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteDishAsync(int id)
        {
            // Get token from cookie
            var token = _httpContextAccessor.HttpContext.Request.Cookies["JWToken"];

            // Clear existing headers to avoid duplicates
            _httpClient.DefaultRequestHeaders.Remove("Authorization");

            // Add it to Authorization header
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/deletemenu/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to delete dish {id}. Status: {response.StatusCode}");
                    return false;
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP error deleting dish {id}: {httpEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting dish {id}: {ex.Message}");
                return false;
            }

        }



        //public async Task<bool> TestAuthHandler()
        //{
        //    Console.WriteLine("=== TESTING AUTH HANDLER ===");

        //    // This request should automatically get the auth token
        //    var response = await _httpClient.GetAsync($"{_baseUrl}/getalldishes");

        //    Console.WriteLine($"Test response: {response.StatusCode}");
        //    Console.WriteLine("=== END TEST ===");

        //    return response.IsSuccessStatusCode;
        //}


    }
}
