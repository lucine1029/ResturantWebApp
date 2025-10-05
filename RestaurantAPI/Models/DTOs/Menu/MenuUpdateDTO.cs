namespace RestaurantAPI.Models.DTOs.Menu
{
    public class MenuUpdateDTO
    {
        public string DishName { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
