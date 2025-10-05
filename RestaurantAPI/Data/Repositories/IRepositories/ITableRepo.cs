
using RestaurantAPI.Models;

namespace RestaurantAPI.Data.Repositories.IRepositories
{
    public interface ITableRepo
    {
        Task<List<Table>> GetAllTablesAsync();
        Task<Table> GetTableByIdAsync(int tableId);
        Task<Table> GetTableByTableNumberAsync(int tableNumber);
        Task<List<Table>> GetAvailableTablesByCapacityAsync(DateTime date, TimeSpan startTime, TimeSpan duration, int numOfGuest);
        Task<int> AddTableAsync(Table table);
        Task<bool> UpdateTableAsync(Table table);
        Task<bool> DeleteTableAsync(int tableId);

    }
}
