namespace RestaurantAPI.Models.DTOs.Booking
{
    public class BookingUpdateDTO
    {
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public int NumberOfGuests { get; set; }
        public string status { get; set; } = "Confirmed";
        //public string SpecialRequests { get; set; }
    }
}
