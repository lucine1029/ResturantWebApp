using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Models.DTOs.Booking
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public int FK_TableId { get; set; }
        public int FK_CustomerId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int NumberOfGuests { get; set; }
        public string Status { get; set; } = "Confirmed";
        //    [MaxLength(200)]
        //    public string SpecialRequests { get; set; }
        //    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        //    public DateTime? UpdatedAt { get; set; }
    }
    //public enum BookingStatus
    //{
    //    Confirmed,
    //    Cancelled,
    //    Pending
    //}

}
