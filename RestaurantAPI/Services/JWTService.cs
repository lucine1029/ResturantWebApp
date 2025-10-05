using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Models;
using RestaurantAPI.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantAPI.Services
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _configuration;
        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string GenerateToken(Admin admin)
        {
            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, admin.Username),
                new Claim(ClaimTypes.Role, admin.Role),
                new Claim("AdminId", admin.Id.ToString())
            });

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),//don't know if needed
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        //public ClaimsPrincipal ValidateToken(string token)
        //{
        //    if (string.IsNullOrEmpty(token))
        //        return null;

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

        //    try
        //    {
        //        var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //            ValidateIssuer = true,
        //            ValidateAudience = true,
        //            ValidIssuer = _configuration["Jwt:Issuer"],
        //            ValidAudience = _configuration["Jwt:Audience"],
        //            ClockSkew = TimeSpan.Zero
        //        }, out SecurityToken validatedToken);

        //        return principal;
        //    }
        //    catch
        //    {
        //        return null; // invalid token
        //    }
        //}
    }
}
