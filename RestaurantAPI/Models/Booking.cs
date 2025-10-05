using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Table")]
        public int FK_TableId { get; set; }
        [ForeignKey("Customer")]
        public int FK_CustomerId { get; set; }
        [ForeignKey(nameof(FK_TableId))]
        public virtual Table Table { get; set; }

        [ForeignKey(nameof(FK_CustomerId))]
        public virtual Customer Customer{ get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int NumberOfGuests { get; set; }
        [MaxLength(50)]
        public string Status { get; set; } = "Confirmed";
        //[MaxLength(200)]
        //public string SpecialRequests { get; set; }
        //public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        //public DateTime? UpdatedAt { get; set; }
    }
}
