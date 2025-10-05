using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? DishName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        public bool IsVegan { get; set; }
        public bool HasNuts { get; set; }
        public bool HasEgg { get; set; }
        public bool HasDairy { get; set; }
        public bool IsSpicy { get; set; }
        public bool IsGlutenFree { get; set; }
    }
}
