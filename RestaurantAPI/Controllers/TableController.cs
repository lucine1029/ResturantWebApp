using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Constants;
using RestaurantAPI.Models.DTOs.Table;
using RestaurantAPI.Services.IServices;
using System.ComponentModel.DataAnnotations;
using ValidationException = RestaurantAPI.Exceptions.ValidationException;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;
        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        [Authorize(Policy = "RequireStaffOrAbove")]
        [HttpGet]
        [Route("/getalltables")]
        public async Task<ActionResult<List<TableDTO>>> GetAllTables()
        {
            var tables = await _tableService.GetAllTablesAsync();
            return Ok(tables);
        }

        [Authorize(Policy = "RequireStaffOrAbove")]
        [HttpGet]
        [Route("/gettablebyid/{id}")]
        public async Task<ActionResult<TableDTO>> GetTableById(int id)
        {
            var table = await _tableService.GetTableByIdAsync(id);
            if (table == null)
            {
                return NotFound();
            }
            return Ok(table);
        }

        [Authorize(Policy = "RequireStaffOrAbove")]
        [HttpGet]
        [Route("/gettablebytablenumber/{tableNumber}")]
        public async Task<ActionResult<TableDTO>> GetTableByTableNumber(int tableNumber)
        {
            var tableByTableNumber = await _tableService.GetTableByTableNumberAsync(tableNumber);
            if (tableByTableNumber == null)
            {
                return NotFound();
            }
            return Ok(tableByTableNumber);  
        }

        [Authorize(Policy = "RequireManagerOrAbove")]
        [HttpPost]
        [Route("/createtable")]
        public async Task<ActionResult<TableCreateDTO>> CreateTable([FromBody]TableCreateDTO tableCreateDTO)
        {
            try
            {
                var newTable = await _tableService.CreateTableAsync(tableCreateDTO);
                newTable.Message = string.Format(ApiMessages.Success.Created, "Table");  //the constant success message for CRUD
                return CreatedAtAction(nameof(GetTableById), new { id = newTable.Id }, newTable );

            }
            catch(ValidationException ex)
            {
                return BadRequest(new
                {
                    error = ex.ErrorMessage,  //the customered message and code is from the tableService
                    code = ex.ErrorCode
                });
            }
            catch (Exception)
            {
                //_logger.LogError(ex, "An error occurred while creating the table.");
                return StatusCode(500, "An error occurred while creating the table.");
            }
        }

        [Authorize(Policy = "RequireSuperAdmin")]
        [HttpDelete]
        [Route("/deletetable/{id}")]
        public async Task<ActionResult> DeleteTable(int id)
        {
            try
            {
                var deletedTable = await _tableService.DeleteTableAsync(id);
                deletedTable.Message = string.Format(ApiMessages.Success.Deleted, "Table");
                return Ok(deletedTable);
            }
            catch(ValidationException ex)
            { 
                return BadRequest(new
                {
                    error = ex.ErrorMessage,  //the customered message and code is from the tableService
                    code = ex.ErrorCode
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the table.");
            }
        }

        [Authorize(Policy = "RequireManagerOrAbove")]
        [HttpPut]
        [Route("/updatetable/{id}")]
        public async Task<ActionResult> UpdateTable(int id, TableUpdateDTO tableUpdateDTO)
        {
            try
            {
                var updatedTable = await _tableService.UpdateTableAsync(id,tableUpdateDTO);
                updatedTable.Message = string.Format(ApiMessages.Success.Updated, "Table");
                return Ok(updatedTable);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    error = ex.ErrorMessage,  //the customered message and code is from the tableService
                    code = ex.ErrorCode
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the table.");
            }
        }
    }
}
