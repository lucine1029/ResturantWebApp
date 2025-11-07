namespace RestaurantWebApp.Models
{
    public class Table
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }
        public List<Booking> Bookings { get; set; } = new();
    }
}
