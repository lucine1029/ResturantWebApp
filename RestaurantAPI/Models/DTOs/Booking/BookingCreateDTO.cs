using RestaurantAPI.Models.DTOs.Customer;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models.DTOs.Booking
{
    public class BookingCreateDTO
    {
        [Required]
        public DateTime BookingDate { get; set;}
        public string DisplayTime { get; set;} = string.Empty;
        [Required]
        public int NumberOfGuests { get; set; }
        //public string SpecialRequests { get; set; }
        [Required]
        public string CustomerFirstName { get; set; }
        [Required] 
        public string CustomerLastName { get; set; }
        [Required]
        public string CustomerPhone { get; set; }
        [Required]
        public string CustomerEmail { get; set; }

    }
}
