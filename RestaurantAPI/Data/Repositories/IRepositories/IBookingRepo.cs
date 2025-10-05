using RestaurantAPI.Models;
using RestaurantAPI.Models.DTOs.Booking;

namespace RestaurantAPI.Data.Repositories.IRepositories
{
    public interface IBookingRepo
    {
        Task<List<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(int id);
        //Task<List<Booking>> GetBookingsByDateAsync(DateTime date);
        //Task<List<Booking>> GetBookingsByCustomerPhoneAsync(string phone);
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<bool> DeleteBookingAsync(int id);
        Task<bool> UpdateBookingAsync(Booking booking);
        //Task<bool> HasOverLappingBookingAsync(int tableId, DateTime startTime, TimeSpan duration);
    }
}
