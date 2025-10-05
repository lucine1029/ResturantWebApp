namespace RestaurantAPI.Models.DTOs.Table
{
    public class TableMessageDTO
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; }

        public string? Message { get; set; } //show the success message
    }
}
