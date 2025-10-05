using RestaurantAPI.Models;
using RestaurantAPI.Data;
using RestaurantAPI.Data.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace RestaurantAPI.Data.Repositories
{
    public class TableRepo : ITableRepo
    {
        private readonly ApplicationDbContext _context;
        public TableRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddTableAsync(Table table)
        {
            await _context.Table.AddAsync(table);
            await _context.SaveChangesAsync();
            return table.Id;
        }

        public async Task<bool> DeleteTableAsync(int tableId) 
        {
            var rowsAffected = await _context.Table
                .Where(t => t.Id == tableId)
                .ExecuteDeleteAsync();
            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateTableAsync(Table table)
        {
            _context.Table.Update(table);
            var result = await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Table>> GetAllTablesAsync()
        {
            var tables = await _context.Table.ToListAsync();
            return tables;
        }

        public async Task<Table> GetTableByIdAsync(int tableId)
        {
            var table = await _context.Table.FirstOrDefaultAsync(t => t.Id == tableId);
            return table;
        }

        public async Task<Table> GetTableByTableNumberAsync(int tableNumber)
        {
            var table = await _context.Table.FirstOrDefaultAsync(t => t.TableNumber == tableNumber);
            return table;
        }

        public async Task<List<Table>> GetAvailableTablesByCapacityAsync(DateTime date, TimeSpan startTime, TimeSpan duration, int numOfGuest)
        {
            //var requestedStart = date.Date.Add(startTime);
            //var requestedEnd = requestedStart.Add(duration);

            //return await _context.Tables
            //    .Where(t => t.Capacity >= partySize)
            //    .OrderBy(t => t.Capacity) // Prefer smallest suitable table
            //    .FirstOrDefaultAsync(t =>
            //        !_context.Bookings.Any(b =>
            //            b.TableId == t.Id &&
            //            b.Status == "Confirmed" && // Or use your BookingStatus enum
            //            b.BookingDate.Date == date.Date &&

            //            // Check if requested time overlaps with 4-hour blocking window
            //            requestedStart < b.StartTime.AddHours(2) &&      // Before blocking ends
            //            requestedEnd > b.StartTime.AddHours(-2)          // After blocking starts
            //        )
        



                    var availableTables = await _context.Table
                .Where (u => u.Capacity >= numOfGuest)
                .ToListAsync();
            return availableTables;
        }
    }
}
