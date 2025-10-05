using RestaurantAPI.Models;
using System.Security.Claims;

namespace RestaurantAPI.Services.IServices
{
    public interface IJWTService
    {
        string GenerateToken(Admin admin);
        //ClaimsPrincipal ValidateToken(string token);
        
    }
}
