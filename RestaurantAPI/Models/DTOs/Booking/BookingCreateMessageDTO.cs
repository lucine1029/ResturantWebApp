namespace RestaurantAPI.Models.DTOs.Booking
{
    public class BookingCreateMessageDTO
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public string BookingDateFormat => BookingDate.ToString("yyyy-MM-dd");
        public TimeSpan StartTime { get; set; }
        public TimeSpan Duration { get; set; }

        //Table Info
        public int TableId { get; set; }
        public int TableNumber { get; set; }
        public int TableCapacity { get; set; }

        //Customer Info
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone {  get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        public string ConfirmationMessage {  get; set; } = string.Empty;
    }
}
