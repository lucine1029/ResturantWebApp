using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RestaurantAPI.Models
{
    public class Table  //Id is for system use only, TableNumber is for restaurant staff use 
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1, 50, ErrorMessage = "Table number must be between 1 and 50")] //where it shows the errormessage on http response? when will do the auto-validation?
        public int TableNumber { get; set; }
        [Required]
        [Range(1, 10, ErrorMessage = "Capacity must be between 1 and 10")]
        public int Capacity { get; set; }
        [Required]
        public bool IsAvailable { get; set; } //don't know if need to use this property??
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
