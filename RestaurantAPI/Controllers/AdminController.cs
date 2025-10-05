using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Constants;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models.DTOs.Admin;
using RestaurantAPI.Models.DTOs.Customer;
using RestaurantAPI.Services;
using RestaurantAPI.Services.IServices;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Authorize(Policy = "RequireSuperAdmin")]
    public class AdminController : ControllerBase
    {

        //AdminController is only for handling admin-related CRUD operations
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        [Route("/getalladmins")]
        public async Task<ActionResult<List<AdminDTO>>> GetAllAdmins()
        {
            var admins = await _adminService.GetAllAdminsAsync();
            return Ok(admins);
        }

        [HttpGet]
        [Route("/getadminbyusername/{username}")]
        public async Task<ActionResult<AdminDTO>> GetAdminByUsername(string username)
        {
            var admin = await _adminService.GetAdminByUsernameAsync(username);
            if (admin == null)
            {
                return NotFound();
            }
            return Ok(admin);
        }

        [HttpPost]
        [Route("/createadmin")]
        public async Task<ActionResult<AdminMessageDTO>> CreateAdmin([FromBody] AdminCreateDTO adminCreateDTO)
        {
            try
            {
                var newAdmin = await _adminService.CreateAdminAsync(adminCreateDTO);
                newAdmin.Message = string.Format(ApiMessages.Success.Created, "Admin");  //the constant success message for CRUD
                return CreatedAtAction(nameof(GetAdminByUsername), new { username = newAdmin.Username }, newAdmin);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    error = ex.ErrorMessage,
                    code = ex.ErrorCode
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the admin user.");
            }
        }

        [HttpDelete]
        [Route("/deleteadmin/{name}")]
        public async Task<ActionResult> Deleteadmin(string name)
        {
            try
            {
                var deletedAdmin = await _adminService.DeleteAdminAsync(name);
                deletedAdmin.Message = string.Format(ApiMessages.Success.Deleted, "Admin");
                return Ok(deletedAdmin);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    error = ex.ErrorMessage,
                    code = ex.ErrorCode
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the admin.");
            }
        }

        [HttpPut]
        [Route("/updateadmin/{id}")]
        public async Task<ActionResult> UpdateAdmin(int id, AdminUpdateDTO adminUpdateDTO)
        {
            try
            {
                var updatedAdmin = await _adminService.UpdateAdminAsync(id, adminUpdateDTO);
                updatedAdmin.Message = string.Format(ApiMessages.Success.Updated, "Admin");
                return Ok(updatedAdmin);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    error = ex.ErrorMessage,
                    code = ex.ErrorCode
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the admin.");
            }
        }

    }
}
