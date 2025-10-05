using RestaurantAPI.Models;

namespace RestaurantAPI.Data.Repositories.IRepositories
{
    public interface IMenuRepo
    {
        Task<List<Menu>> GetAllDishesAsync();
        Task<Menu> GetDishByIdAsync(int menuId);
        Task<Menu> GetDishByDishNameAsync( string dishName);
        Task<int> AddMenuAsync(Menu menu);
        Task<bool> UpdateMenuAsync(Menu menu);
        Task<bool> DeleteMenuAsync(int menuId);
    }
}
