using RestaurantAPI.Models.DTOs.Admin;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; } //for security reasons only input, never output
        [Required]
        public string Role { get; set; }
    }
}
