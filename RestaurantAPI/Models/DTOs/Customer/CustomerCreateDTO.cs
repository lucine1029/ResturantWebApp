using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models.DTOs.Customer
{
    public class CustomerCreateDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
