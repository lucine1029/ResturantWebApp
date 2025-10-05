using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Constants
{
    public class ResturantConfig
    {
        //Resturant info.
        //public static int Id { get; set; }
        //public static string Name { get; set; }
        //public static string Address { get; set; }
        //public static string Phone { get; set; }
        //public static string Email { get; set; }

        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public TimeSpan DefaultBookingDuration { get; set; }
        public TimeSpan BookingSlotInterval { get; set; }
       
    }
}
