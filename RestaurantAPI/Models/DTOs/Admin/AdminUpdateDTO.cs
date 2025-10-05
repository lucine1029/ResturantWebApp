namespace RestaurantAPI.Models.DTOs.Admin
{
    public class AdminUpdateDTO
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string NewPassword { get; set; } //optional or separate
    }
}
