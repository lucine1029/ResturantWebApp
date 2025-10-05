namespace RestaurantAPI.Models.DTOs.Customer
{
    public class CustomerMessageDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Message {  get; set; }
    }
}
