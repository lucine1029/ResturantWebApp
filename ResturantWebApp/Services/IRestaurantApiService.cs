using RestaurantWebApp.Models;

namespace RestaurantWebApp.Services
{
    public interface IRestaurantApiService
    {
        Task<Menu> GetDishByIdAsync(int id);
        Task<List<Menu>> GetAllDishesAsync();
        Task<string> LoginAsync(LoginViewModel loginViewModel);
        Task<bool> LogoutAsync(string token);
        Task<bool> AddDishAsync(DishViewModel newDish);
        Task<bool> UpdateDishAsync(int id, Menu dish);

    }
}
