namespace RestaurantAPI.Models.DTOs.Booking
{
    public class BookingCheckDTO
    {
        public int BookingId { get; set; } 
        public DateTime BookingDate { get; set; }
        public string DisplayTime { get; set; }
        public int NumberOfGuests { get; set; }
        public string? Message { get; set; } //show the success message

    }
}
