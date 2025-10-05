namespace RestaurantAPI.Models.DTOs.Table
{
    public class TableStatusDTO
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }
    }
}
