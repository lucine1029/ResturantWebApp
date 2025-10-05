using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data.Repositories.IRepositories;
using RestaurantAPI.Models;

namespace RestaurantAPI.Data.Repositories
{
    public class AdminRepo : IAdminRepo
    {
        private readonly ApplicationDbContext _context;
        public AdminRepo(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<int> AddAdminAsync(Admin admin)
        {
            await _context.Admin.AddAsync(admin);
            await _context.SaveChangesAsync();
            return admin.Id;
        }

        public async Task<bool> DeleteAdminAsync(int adminId)
        {
            var rowsAffected = await _context.Admin
                .Where(t => t.Id == adminId)
                .ExecuteDeleteAsync();
            if (rowsAffected > 0)
                {
                return true;
            }
            return false;
        }

        public async Task<Admin> GetAdminByUsernameAsync(string name)
        {
            var admin = await _context.Admin.FirstOrDefaultAsync(t => t.Username == name);
            return admin;
        }

        public async Task<List<Admin>> GetAllAdminsAsync()
        {
            var admins = await _context.Admin.ToListAsync();
            return admins;
        }

        public async Task<bool> UpdateAdminAsync(Admin admin)
        {
            _context.Admin.Update(admin);
            var result = await _context.SaveChangesAsync();
            return true;
        }
    }
}
