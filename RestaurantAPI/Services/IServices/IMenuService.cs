using RestaurantAPI.Models;
using RestaurantAPI.Models.DTOs.Menu;

namespace RestaurantAPI.Services.IServices
{
    public interface IMenuService
    {
        Task<List<MenuMessageDTO>> GetAllDishesAsync();
        Task<MenuDTO> GetDishByIdAsync(int menuId);
        Task<MenuMessageDTO> CreateMenuAsync(MenuCreateDTO menuCreateDTO);
        Task<MenuMessageDTO> UpdateMenuAsync(int menuId, MenuUpdateDTO menuUpdateDto);
        Task<MenuMessageDTO> DeleteMenuAsync(int menuId);
    }
}
