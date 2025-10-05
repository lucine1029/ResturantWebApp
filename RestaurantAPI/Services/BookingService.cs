using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using RestaurantAPI.Constants;
using RestaurantAPI.Data.Repositories.IRepositories;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using RestaurantAPI.Models.DTOs.Booking;
using RestaurantAPI.Models.DTOs.Customer;
using RestaurantAPI.Models.DTOs.Table;
using RestaurantAPI.Services.IServices;

namespace RestaurantAPI.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepo _bookingRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly ITableRepo _tableRepo;
        private readonly ResturantConfig _resturantConfiguration;

        public BookingService(IBookingRepo bookingRepo,
            ICustomerRepo customerRepo,
            ITableRepo tableRepo,
            IOptions<ResturantConfig> resturantConfiguration)
        {
            _bookingRepo = bookingRepo;
            _customerRepo = customerRepo;
            _tableRepo = tableRepo;
            _resturantConfiguration = resturantConfiguration.Value;
        }


        public async Task<List<AvailableTimeSlotsDTO>> GetAvailableTimeSlotsAsync(DateTime date, int numberOfGuests) //slots first, then check availability
        {
            var availableSlots = new List<AvailableTimeSlotsDTO>();

            //1. Generate all possible slots for the day

            var openingTime = date.Date.Add(_resturantConfiguration.OpeningTime);
            var closingTime = date.Date.Add(_resturantConfiguration.ClosingTime);

            for (var slotStartTime = openingTime; slotStartTime <= closingTime; slotStartTime = slotStartTime.Add(_resturantConfiguration.BookingSlotInterval))
            {
                availableSlots.Add(new AvailableTimeSlotsDTO
                {
                    DisplayTime = slotStartTime.ToString("HH:mm"),
                    StartTime = slotStartTime.TimeOfDay,
                });
            }

            //2. Check availability for each time slot
            foreach (var slot in availableSlots)
            {
                var availableTables = await _tableRepo.GetAvailableTablesByCapacityAsync
                    (date, slot.StartTime, _resturantConfiguration.DefaultBookingDuration, numberOfGuests
                    );
                slot.IsAvailable = availableTables != null;
            }
            return availableSlots;
        }

        public async Task<List<BookingDTO>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepo.GetAllBookingsAsync();

            var bookingList = new List<BookingDTO>();
            foreach (var booking in bookings)
            {
                var bookingDto = new BookingDTO
                {
                    Id = booking.Id,
                    FK_TableId = booking.FK_TableId,
                    FK_CustomerId = booking.FK_CustomerId,
                    BookingDate = booking.BookingDate,
                    StartTime = booking.StartTime,
                    Duration = booking.Duration,
                    NumberOfGuests = booking.NumberOfGuests
                };
                bookingList.Add(bookingDto);
            }
            return bookingList;
        }

        public async Task<BookingDTO> GetBookingByIdAsync(int id)
        {
            var booking = await _bookingRepo.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return null;
            }
            var bookingDto = new BookingDTO
            {
                Id = booking.Id,
                FK_TableId = booking.FK_TableId,
                FK_CustomerId = booking.FK_CustomerId,
                BookingDate = booking.BookingDate,
                StartTime = booking.StartTime,
                Duration = booking.Duration,
                NumberOfGuests = booking.NumberOfGuests
            };
            return bookingDto;
        }

        //public async Task<List<BookingDTO>> GetBookingsByDateAsync(DateTime date)
        //{
        //    throw new NotImplementedException();
        //}
        public async Task<List<TableDTO>> GetAvailableTablesAsync(BookingCheckDTO bookingCheckDTO)
        {
            var bookingStart = TimeSpan.Parse(bookingCheckDTO.DisplayTime);
            var bookingEnd = bookingStart.Add(_resturantConfiguration.DefaultBookingDuration);

            //check available tables again incase someone booked it before booking confirmation
            var availableTables = await _tableRepo.GetAvailableTablesByCapacityAsync(
                bookingCheckDTO.BookingDate,
                bookingStart,
                _resturantConfiguration.DefaultBookingDuration,
                bookingCheckDTO.NumberOfGuests
                );

            var availableTableList = new List<TableDTO>();
            if (availableTables == null || availableTables.Count == 0)
            {
                //no available table found
                return null;
            }
            foreach (var availableTable in availableTables)
            {
                var tableDto = new TableDTO
                {
                    Id = availableTable.Id,
                    TableNumber = availableTable.TableNumber,
                    Capacity = availableTable.Capacity
                };
                Console.WriteLine($"Available Table: {tableDto.TableNumber} with capacity {tableDto.Capacity}"); //for debugging
                availableTableList.Add(tableDto);
            }
            return availableTableList;
        }


        public async Task<BookingCreateMessageDTO> CreateBookingAsync(BookingCreateDTO bookingCreateDTO)
        {
            var bookingStart = TimeSpan.Parse(bookingCreateDTO.DisplayTime);
            var bookingEnd = bookingStart.Add(_resturantConfiguration.DefaultBookingDuration);

            //check available tables again incase someone booked it before booking confirmation
            var availableTables = await _tableRepo.GetAvailableTablesByCapacityAsync(
                bookingCreateDTO.BookingDate,
                bookingStart,
                _resturantConfiguration.DefaultBookingDuration,
                bookingCreateDTO.NumberOfGuests
                );

            var selectedTable = availableTables  //my logic is to get the smallest available table that fits the party size
                    .OrderBy(t => t.Capacity)
                    .FirstOrDefault();

            //find or create customer
            var customer = await _customerRepo.GetCustomerByPhoneNumberAsync(bookingCreateDTO.CustomerPhone);
            if (customer == null)
            {
                customer = new Customer
                {
                    FirstName = bookingCreateDTO.CustomerFirstName,
                    LastName = bookingCreateDTO.CustomerLastName,
                    Email = bookingCreateDTO.CustomerEmail,
                    Phone = bookingCreateDTO.CustomerPhone
                };
                await _customerRepo.AddCustomerAsync(customer);
            }

            //create booking
            var booking = new Models.Booking
            {
                FK_TableId = selectedTable.Id,
                FK_CustomerId = customer.Id,
                BookingDate = bookingCreateDTO.BookingDate,
                StartTime = bookingStart,
                Duration = _resturantConfiguration.DefaultBookingDuration,
                Status = "Confirmed",
                NumberOfGuests = bookingCreateDTO.NumberOfGuests,
            };
            await _bookingRepo.CreateBookingAsync(booking);

            //booking confirmed
            var bookingConfirmation = new BookingCreateMessageDTO
            {
                BookingId = booking.Id,
                BookingDate = booking.BookingDate,
                StartTime = booking.StartTime,
                Duration = booking.Duration,
                TableId = selectedTable.Id,
                TableNumber = selectedTable.TableNumber,
                TableCapacity = selectedTable.Capacity,
                CustomerId = customer.Id,
                CustomerName = $"{bookingCreateDTO.CustomerFirstName} {bookingCreateDTO.CustomerLastName}",
                CustomerPhone = bookingCreateDTO.CustomerPhone,
                CustomerEmail = bookingCreateDTO.CustomerEmail,
                ConfirmationMessage = $"Your booking is confirmed for {booking.StartTime.ToString(@"hh\:mm")} on {booking.BookingDate:yyyy-MM-dd} , you will soon get an confirmation email."
            };
            return bookingConfirmation;
        }

        public async Task<BookingCheckDTO> DeleteBookingAsync(int bookingId)
        {
            //1. Check if the booking exists
            var booking = await _bookingRepo.GetBookingByIdAsync(bookingId);
            if (booking == null)
            {
                throw new ValidationException
                    (
                    errorMessage: $"Booking Id '{bookingId}' is not exist!",
                    errorCode: "Booking.NotFound"
                    );
            }
            //3. If all passed, then delete
            bool wasDeleted = await _bookingRepo.DeleteBookingAsync(bookingId);
            //4. check if repository/database deleted success or not
            if (!wasDeleted)
            {
                throw new Exception("Failed to delete the booking due to unexpected error.");
            }
            //5. Map back to the DTO
            return new BookingCheckDTO
            {
                BookingId = bookingId,
                BookingDate = booking.BookingDate,
                DisplayTime = booking.StartTime.ToString(@"hh\:mm"),
                NumberOfGuests = booking.NumberOfGuests
            };
        }
        public async Task<BookingCheckDTO> UpdateBookingAsync(int id, BookingUpdateDTO bookingUpdateDTO)
        {
            //1.  get an existing booking
            var existingBooking = await _bookingRepo.GetBookingByIdAsync(id);
            if (existingBooking == null)
            {
                throw new ValidationException
                    (
                    errorMessage: $"Booking Id '{id}' is not exist!",
                    errorCode: "Booking.NotFound"
                    );
            }
            //3. Modify
            existingBooking.BookingDate = bookingUpdateDTO.BookingDate;
            existingBooking.StartTime = bookingUpdateDTO.StartTime;
            existingBooking.NumberOfGuests = bookingUpdateDTO.NumberOfGuests;
            //4. Save
            var updatedBooking = await _bookingRepo.UpdateBookingAsync(existingBooking);
            //5. Map and return 
            return new BookingCheckDTO
            {
                BookingId = existingBooking.Id,
                BookingDate = bookingUpdateDTO.BookingDate,
                DisplayTime = bookingUpdateDTO.StartTime.ToString(@"hh\:mm"),
                NumberOfGuests = bookingUpdateDTO.NumberOfGuests
            };
        }


        //public async Task<List<BookingDTO>> GetBookingsByCustomerPhoneAsync(string phone)
        //{
        //    var tables = await _tableRepo.GetAllTablesAsync();
        //    var tableList = new List<TableDTO>();
        //    foreach (var table in tables)
        //    {
        //        var tableDto = new TableDTO
        //        {
        //            Id = table.Id,
        //            TableNumber = table.TableNumber,
        //            Capacity = table.Capacity
        //        };
        //        tableList.Add(tableDto);
        //    }
        //    return tableList;
        //    //var bookings = await _bookingRepo.GetBookingsByCustomerPhoneAsync(phone);
        //    //if (bookings == null || bookings.Count == 0)
        //    //{
        //    //    return null;
        //    //}
        //    //var bookingList = bookings.OrderByDescending(b => b.BookingDate).ThenByDescending(b => b.StartTime).FirstOrDefault();
        //    //var bookingDto = new BookingDTO
        //    //{
        //    //    Id = bookingList.Id,
        //    //    FK_TableId = bookingList.FK_TableId,
        //    //    FK_CustomerId = bookingList.FK_CustomerId,
        //    //    BookingDate = bookingList.BookingDate,
        //    //    StartTime = bookingList.StartTime,
        //    //    Duration = bookingList.Duration,
        //    //    NumberOfGuests = bookingList.NumberOfGuests
        //    //};
        //    //return bookingList;
        //}
    }
}
