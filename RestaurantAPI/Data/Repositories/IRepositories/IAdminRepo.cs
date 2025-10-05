using RestaurantAPI.Models;

namespace RestaurantAPI.Data.Repositories.IRepositories
{
    public interface IAdminRepo
    {
        Task<List<Admin>> GetAllAdminsAsync();
        Task<Admin> GetAdminByUsernameAsync(string username);
        Task<int> AddAdminAsync(Admin admin);
        Task<bool> UpdateAdminAsync(Admin admin);
        Task<bool> DeleteAdminAsync(int adminId);
    }
}
