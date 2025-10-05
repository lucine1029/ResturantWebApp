namespace RestaurantAPI.Models.DTOs.Menu
{
    public class MenuMessageDTO
    {
        public int Id { get; set; }
        public string DishName { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }

        public string? Message {  get; set; }
    }
}
