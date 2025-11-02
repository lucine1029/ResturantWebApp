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

        public async Task<bool> TestAuthHandler()
        {
            Console.WriteLine("=== TESTING AUTH HANDLER ===");

            // This request should automatically get the auth token
            var response = await _httpClient.GetAsync($"{_baseUrl}/getalldishes");

            Console.WriteLine($"Test response: {response.StatusCode}");
            Console.WriteLine("=== END TEST ===");

            return response.IsSuccessStatusCode;
        }
    }
}
