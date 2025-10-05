using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantAPI.Data.Repositories.IRepositories;
using RestaurantAPI.Models;

namespace RestaurantAPI.Data.Repositories
{
    public class MenuRepo : IMenuRepo
    {
        private readonly ApplicationDbContext _context;
        public MenuRepo(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<int> AddMenuAsync(Menu menu)
        {
            await _context.Menu.AddAsync(menu);
            await _context.SaveChangesAsync();
            return menu.Id;
        }

        public async Task<bool> DeleteMenuAsync(int menuId)
        {
            var rowsAffected = await _context.Menu
                .Where(t => t.Id == menuId)
                .ExecuteDeleteAsync();
            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateMenuAsync(Menu menu)
        {
            _context.Menu.Update(menu);
            var result = await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Menu>> GetAllDishesAsync()
        {
            var dishes = await _context.Menu.ToListAsync();
            return dishes;
        }

        public async Task<Menu> GetDishByIdAsync(int menuId)
        {
            var dish = await _context.Menu.FirstOrDefaultAsync(t => t.Id == menuId);
            return dish;
        }

        public async Task<Menu> GetDishByDishNameAsync(string dishName)
        {
            var dish = await _context.Menu.FirstOrDefaultAsync(t => t.DishName == dishName);
            return dish;
        }
    }
}
