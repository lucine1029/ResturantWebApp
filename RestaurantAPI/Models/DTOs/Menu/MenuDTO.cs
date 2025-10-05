using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models.DTOs.Menu
{
    public class MenuDTO
    {
        public int Id { get; set; }
        public string DishName { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsVegan { get; set; }
        public bool HasNuts { get; set; }
        public bool HasEgg { get; set; }
        public bool HasDairy { get; set; }
        public bool IsSpicy { get; set; }
        public bool IsGlutenFree { get; set; }
    }
}
