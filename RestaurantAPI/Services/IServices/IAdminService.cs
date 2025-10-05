using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using RestaurantAPI.Models;
using RestaurantAPI.Models.DTOs.Admin;
using RestaurantAPI.Models.DTOs.Customer;

namespace RestaurantAPI.Services.IServices
{
    public interface IAdminService
    {
        Task<Admin> AuthenticateAsync(string username, string password); //here is return admin not adminDTO for generating token 
        Task<List<AdminDTO>> GetAllAdminsAsync();
        Task<AdminDTO> GetAdminByUsernameAsync(string username);
        Task<AdminMessageDTO> CreateAdminAsync(AdminCreateDTO adminCreateDTO);
        Task<AdminMessageDTO> UpdateAdminAsync(int adminId, AdminUpdateDTO adminUpdateDto);
        Task<AdminMessageDTO> DeleteAdminAsync(string username);
    }
}
