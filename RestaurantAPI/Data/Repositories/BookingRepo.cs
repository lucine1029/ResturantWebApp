using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantAPI.Data.Repositories.IRepositories;
using RestaurantAPI.Models;
using RestaurantAPI.Models.DTOs.Booking;

namespace RestaurantAPI.Data.Repositories
{
    public class BookingRepo : IBookingRepo
    {
        private readonly ApplicationDbContext _context;
        public BookingRepo(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            var bookings = await _context.Booking.ToListAsync();
            return bookings;
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            var booking = await _context.Booking.FirstOrDefaultAsync(b => b.Id == id);
            return booking;
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            await _context.Booking.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<bool> DeleteBookingAsync(int bookingId)
        {
            var rowsAffected = await _context.Booking
                .Where(t => t.Id == bookingId)
                .ExecuteDeleteAsync();
            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }


        public async Task<bool> UpdateBookingAsync(Booking booking)
        {
            _context.Booking.Update(booking);
            var result = await _context.SaveChangesAsync();
            return true;
        }


        //public async Task<List<Booking>> GetBookingsByCustomerPhoneAsync(string phone)
        //{
        //    var bookings = await _context.Booking
        //        .Include(b => b.Customer)
        //        .Where(b => b.Customer.Phone == phone)
        //        .ToListAsync();
        //    return bookings;
        //}

        //public async Task<List<Booking>> GetBookingsByDateAsync(DateTime date)
        //{
        //    var bookings = await _context.Booking
        //        .Where(b => b.BookingDate.Date == date.Date)
        //        .ToListAsync();
        //    return bookings;
        //}

        //public async Task<bool> HasOverLappingBookingAsync(int tableId, DateTime startTime, TimeSpan duration)
        //{
        //    throw new NotImplementedException();
        //}


    }
}
