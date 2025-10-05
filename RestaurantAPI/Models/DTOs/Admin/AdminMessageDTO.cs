namespace RestaurantAPI.Models.DTOs.Admin
{
    public class AdminMessageDTO
    {
        public int Id { get; set; }
        public string Username{ get; set; }
        public string Role { get; set; }
        public string? Message { get; set; }
    }
}
