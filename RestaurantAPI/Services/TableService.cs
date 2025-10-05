using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using RestaurantAPI.Constants;
using RestaurantAPI.Data.Repositories.IRepositories;
using RestaurantAPI.Models.DTOs.Booking;
using RestaurantAPI.Models.DTOs.Table;
using RestaurantAPI.Services.IServices;
using System;
using ValidationException = RestaurantAPI.Exceptions.ValidationException;

namespace RestaurantAPI.Services
{
    public class TableService : ITableService
    {
        //DI injection of ITableRepo
        private readonly ITableRepo _tableRepo;
        private readonly ResturantConfig _resturantConfiguration;
        public TableService(ITableRepo tableRepo, IOptions<ResturantConfig> resturantConfiguration)
        {
            _tableRepo = tableRepo;
            _resturantConfiguration = resturantConfiguration.Value;
        }


        public async Task<List<TableDTO>> GetAllTablesAsync()
        {
            var tables = await _tableRepo.GetAllTablesAsync();
            var tableList = new List<TableDTO>();
            foreach (var table in tables)
            {
                var tableDto = new TableDTO
                {
                    Id = table.Id,
                    TableNumber = table.TableNumber,
                    Capacity = table.Capacity
                };
                tableList.Add(tableDto);
            }
            return tableList;
        }

        public async Task<TableDTO> GetTableByIdAsync(int tableId)
        {
            var table = await _tableRepo.GetTableByIdAsync(tableId);
            if (table == null)
            {
                return null;
            }
            var tableDto = new TableDTO
            {
                Id = table.Id,
                TableNumber = table.TableNumber,
                Capacity = table.Capacity
            };
            return tableDto;
        }

        public async Task<TableDTO> GetTableByTableNumberAsync(int tableNumber)
        {
            var tableByTableNumber = await _tableRepo.GetTableByTableNumberAsync(tableNumber);
            if (tableByTableNumber == null)
            {
                return null;
            }
            var tableDto = new TableDTO
            {
                Id = tableByTableNumber.Id,
                TableNumber = tableByTableNumber.TableNumber,
                Capacity = tableByTableNumber.Capacity
            };
            return tableDto;
        }

        //public async Task<List<TableDTO>> GetAvailableTablesAsync(int numOfGuest, DateTime requiredStartTime, TimeSpan duration)
        //{
        //    var requiredEndTime = requiredStartTime.Add();
        //    var availableTables = await _tableRepo.GetAvailableTablesAsync(numOfGuest, requiredStartTime, requiredEndTime);
        //    if (availableTables == null)
        //    {
        //        return null;
        //    }
        //    var tableDto = new TableDTO
        //    {
        //        Id = availableTables.Id,
        //        TableNumber = availableTables.TableNumber,
        //        Capacity = availableTables.Capacity
        //    };
        //    return tableDto;
        //}

        public async Task<TableMessageDTO> CreateTableAsync(TableCreateDTO tableCreateDTO)
        {
            //1. Input validation on the DTO early in the service layer than mapping to entity
            var existingTable = await _tableRepo.GetTableByTableNumberAsync(tableCreateDTO.TableNumber);
            if (existingTable != null)
            {
                throw new ValidationException(
                    errorMessage: $"Table number '{tableCreateDTO.TableNumber}' has already exist!",
                    errorCode: "Table.DuplicateNumber"
                    );
            }
            if (tableCreateDTO.Capacity <= 0 || tableCreateDTO.Capacity > 10)
            {
                throw new ValidationException(
                    errorMessage: "Capacity must be between 1 and 10 !",
                    errorCode: "Table.InvalidCapacity"
                    );
            }
            //2.manually Map DTO to entity, here is new Table instead of TableDTO!!?? Can we use IMapper to auto-mapping?
            var newTable = new Models.Table
            {
                TableNumber = tableCreateDTO.TableNumber,
                Capacity = tableCreateDTO.Capacity
            };
            //3. Call the repository method to add the table
            var newTableId = await _tableRepo.AddTableAsync(newTable);
            //4. Map the saved Entity back to a TableDTO to return ??
            var tableMessageDTO = new TableMessageDTO
            {
                Id = newTable.Id,
                TableNumber = newTable.TableNumber,
                Capacity = newTable.Capacity
            };
            return tableMessageDTO;
        }

        public async Task<TableMessageDTO> DeleteTableAsync(int tableId)
        {
            //1. Check if the table exists
            var table = await _tableRepo.GetTableByIdAsync(tableId);
            if (table == null)
            {
                throw new ValidationException
                    (
                    errorMessage: $"Table Id '{tableId}' is not exist!",
                    errorCode: "Table.NotFound"
                    );
            }
            //2. Business rule validation,check if the table is occupied or has future reservation
            if (table.IsAvailable)
            {
                throw new ValidationException
                    (
                    errorMessage: $"{table.TableNumber}' is occupied/booked, can not be deleted!",
                    errorCode: "Table.CannotDelete"
                    );
            }
            //3. If all passed, then delete
            bool wasDeleted = await _tableRepo.DeleteTableAsync(tableId);
            //4. check if repository/database deleted success or not
            if (!wasDeleted)
            {
                throw new Exception("Failed to delete the talbe due to unexpected error.");
            }
            //5. Map back to the DTO
            return new TableMessageDTO
            {
                Id = table.Id,
                TableNumber = table.TableNumber,
                Capacity = table.Capacity
            };
        }

        public async Task<TableMessageDTO> UpdateTableAsync(int tableId, TableUpdateDTO tableUpdateDTO)
        {
            //1.  get an existing table
            var existingTable = await _tableRepo.GetTableByIdAsync(tableId);
            if (existingTable == null)
            {
                throw new ValidationException
                    (
                    errorMessage: $"Table Id '{tableId}' is not exist!",
                    errorCode: "Table.NotFound"
                    );
            }
            //2. Validation of business rules check the updated capacity, here I assued the TableNumber/Id can not be changed
            if (tableUpdateDTO.Capacity <= 0 || tableUpdateDTO.Capacity > 10)
            {
                throw new ValidationException(
                    errorMessage: "Capacity must be between 1 and 10 !",
                    errorCode: "Table.InvalidCapacity"
                    );
            }
            //3. Modify
            existingTable.Capacity = tableUpdateDTO.Capacity;
            //4. Save
            var updatedTable = await _tableRepo.UpdateTableAsync(existingTable);
            //5. Map and return 
            return new TableMessageDTO
            {
                Id = existingTable.Id, //has not changed
                TableNumber = existingTable.TableNumber, //has not changed
                Capacity = tableUpdateDTO.Capacity
            };
        }
    }
}
