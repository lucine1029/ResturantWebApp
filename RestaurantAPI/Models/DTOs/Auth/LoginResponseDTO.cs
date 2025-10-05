namespace RestaurantAPI.Models.DTOs.Auth
{
    public class LoginResponseDTO
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
