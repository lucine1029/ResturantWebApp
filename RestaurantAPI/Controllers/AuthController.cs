using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Data.Repositories.IRepositories;
using RestaurantAPI.Models.DTOs.Auth;
using RestaurantAPI.Services.IServices;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //AuthController is only for handling authentication-related endpoints like login, register, token refresh, etc.It calls for two services
        private readonly IAdminService _adminService;
        private readonly IJWTService _jwtService;
        public AuthController(IAdminService adminService, IJWTService jwtService)
        {
           _adminService = adminService;
            _jwtService = jwtService;
        }


        [HttpPost]
        [Route("/login")]
        public async Task<ActionResult> Login([FromQuery] LoginDTO loginDTO)
        {
            var admin = await _adminService.AuthenticateAsync(loginDTO.Username, loginDTO.Password);
            if (admin == null) 
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }
            var token = _jwtService.GenerateToken(admin);//generate JWT using the full entity
            var loginResponse = new LoginResponseDTO //return a DTO in the response
            {

                Token = token,
                Username = admin.Username,
                Role = admin.Role

            };
            return Ok(new { token, loginResponse});
        }
    }
}
