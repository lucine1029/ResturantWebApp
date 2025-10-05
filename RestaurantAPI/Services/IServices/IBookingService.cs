using RestaurantAPI.Models.DTOs.Booking;
using RestaurantAPI.Models.DTOs.Table;

namespace RestaurantAPI.Services.IServices
{
    public interface IBookingService
    {
        //for customer-related
        Task<List<AvailableTimeSlotsDTO>> GetAvailableTimeSlotsAsync(DateTime date, int numberOfGuests);
        Task<BookingCreateMessageDTO> CreateBookingAsync(BookingCreateDTO bookingCreateDTO);


        //for admin-only
        Task<List<BookingDTO>> GetAllBookingsAsync();
        Task<BookingDTO> GetBookingByIdAsync (int id);
        Task<List<TableDTO>> GetAvailableTablesAsync(BookingCheckDTO bookingCheckDTO);
        //Task<List<BookingDTO>> GetBookingsByCustomerPhoneAsync(string phone);
        //Task<List<BookingDTO>> GetBookingsByDateAsync(DateTime date);
        Task<BookingCheckDTO> UpdateBookingAsync(int id, BookingUpdateDTO bookingUpdateDTO);
        Task<BookingCheckDTO> DeleteBookingAsync(int id);

    }
}
