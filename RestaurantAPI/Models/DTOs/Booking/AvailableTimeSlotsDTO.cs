namespace RestaurantAPI.Models.DTOs.Booking
{
    public class AvailableTimeSlotsDTO
    {
        public TimeSpan StartTime { get; set; }
        public string DisplayTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}
