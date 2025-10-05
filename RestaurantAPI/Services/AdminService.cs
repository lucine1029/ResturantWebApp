using RestaurantAPI.Data.Repositories;
using RestaurantAPI.Data.Repositories.IRepositories;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using RestaurantAPI.Models.DTOs.Admin;
using RestaurantAPI.Models.DTOs.Customer;
using RestaurantAPI.Services.IServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace RestaurantAPI.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepo _adminRepo;
        public AdminService(IAdminRepo adminRepo)
        {
            _adminRepo = adminRepo;
        }

        public async Task<Admin> AuthenticateAsync(string username, string password)
        {
            //1. Check if the admin exists
            var admin = await _adminRepo.GetAdminByUsernameAsync(username);
            //2. Verify password
            if (admin == null || !BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
            {
                //invalid username or password
                return null;
            }
            //3. Return entity for generating token not AdminDTO, we will response the front by returning DTO in the controller
            return admin;

        }

        public async Task<AdminMessageDTO> CreateAdminAsync(AdminCreateDTO adminCreateDTO)
        {
            //1.Input validation
            var existingAdmin = await _adminRepo.GetAdminByUsernameAsync(adminCreateDTO.Username);
            if (existingAdmin != null)
            {
                throw new Exception($"Admin with username {adminCreateDTO.Username} already exists!");
            }
            //can valid role as well, do it later
            //2.Hash password, Map DTO to entity
            var newAdmin = new Models.Admin
            {
                Username = adminCreateDTO.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminCreateDTO.Password), //hash password
                Role = adminCreateDTO.Role
            };

            //3.Call the repository method to add the admin
            var newAdminId = await _adminRepo.AddAdminAsync(newAdmin);
            //4. Map the saved Entity back to a AdminDTO
            return new AdminMessageDTO
            {
                Id = newAdmin.Id,
                Username = newAdmin.Username,
                Role = newAdmin.Role
            };
        }


        public async Task<AdminMessageDTO> DeleteAdminAsync(string username)
        {
            //1. Check if the admin exists
            var admin = await _adminRepo.GetAdminByUsernameAsync(username);
            if (admin == null)
            {
                throw new ValidationException
                    (
                    errorMessage: $"Admin Name '{username}' is not exist!",
                    errorCode: "Admin.NotFound"
                    );
            }
            //3. If all passed, then delete
            bool wasDeleted = await _adminRepo.DeleteAdminAsync(admin.Id);
            //4. check if repository/database deleted success or not
            if (!wasDeleted)
            {
                throw new Exception("Failed to delete the admin due to unexpected error.");
            }
            //5. Map back to the DTO
            return new AdminMessageDTO
            {
                Id = admin.Id,
                Username = admin.Username,
                Role = admin.Role
            };
        }

        public async Task<AdminDTO> GetAdminByUsernameAsync(string username)
        {
            var admin = await _adminRepo.GetAdminByUsernameAsync(username);
            if (admin == null)
            {
                return null;
            }
            var adminDTO = new AdminDTO
            {
                Id = admin.Id,
                Username = admin.Username,
                Role = admin.Role
            };
            return adminDTO;
        }

        public async Task<List<AdminDTO>> GetAllAdminsAsync() //never show password
        {
            var admins = await _adminRepo.GetAllAdminsAsync();
            var adminList = new List<AdminDTO>();
            foreach (var a in admins)
            {
                var adminDTO = new AdminDTO
                {
                    Id = a.Id,
                    Username = a.Username,
                    Role = a.Role
                };
                adminList.Add(adminDTO);
            }
            return adminList;
        }

        public async Task<AdminMessageDTO> UpdateAdminAsync(int adminId, AdminUpdateDTO adminUpdateDto)
        {
            //1.  get an existing admin
            var existingAdmin = await _adminRepo.GetAdminByUsernameAsync(adminUpdateDto.Username);
            if (existingAdmin == null || existingAdmin.Id != adminId)
            {
                throw new ValidationException
                    (
                    errorMessage: $"Admin Name '{existingAdmin.Username}' is not exist!",
                    errorCode: "Admin.NotFound"
                    );
            }
            //3. Modify
            existingAdmin.Username = adminUpdateDto.Username;
            existingAdmin.Role = adminUpdateDto.Role;
            if (!string.IsNullOrEmpty(adminUpdateDto.NewPassword))
            {
                existingAdmin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminUpdateDto.NewPassword); //hash password
            }
            //4. Save
            var updatedTable = await _adminRepo.UpdateAdminAsync(existingAdmin);
            //5. Map and return
            return new AdminMessageDTO
            {
                Id = existingAdmin.Id, //has not changed
                Username = existingAdmin.Username,
                Role = existingAdmin.Role
            };
        }
    }
}
