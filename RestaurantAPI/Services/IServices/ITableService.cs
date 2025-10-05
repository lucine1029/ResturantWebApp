using RestaurantAPI.Models.DTOs.Booking;
using RestaurantAPI.Models.DTOs.Table;

namespace RestaurantAPI.Services.IServices
{
    public interface ITableService
    {
        Task<List<TableDTO>> GetAllTablesAsync();
        Task<TableDTO> GetTableByIdAsync(int tableId);
        Task<TableDTO> GetTableByTableNumberAsync(int tableNumber);
        //Task<List<TableDTO>> GetAvailableTablesAsync(int numOfGuest, DateTime requiredStartTime, TimeSpan duration);
        Task<TableMessageDTO> CreateTableAsync(TableCreateDTO tableCreateDTO);
        Task<TableMessageDTO> UpdateTableAsync(int tableId, TableUpdateDTO tableUpdateDto);
        Task<TableMessageDTO> DeleteTableAsync(int tableId);
    }
}
