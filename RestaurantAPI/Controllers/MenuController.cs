using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Constants;
using RestaurantAPI.Models.DTOs.Menu;
using RestaurantAPI.Models.DTOs.Table;
using RestaurantAPI.Services;
using RestaurantAPI.Services.IServices;
using ValidationException = RestaurantAPI.Exceptions.ValidationException;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [Authorize(Policy = "RequireStaffOrAbove")]
        [HttpGet]
        [Route("/getalldishes")]
        public async Task<ActionResult<List<MenuDTO>>> GetAllDishes()
        {
            var dishes = await _menuService.GetAllDishesAsync();
            return Ok(dishes);
        }

        [Authorize(Policy = "RequireStaffOrAbove")]
        [HttpGet]
        [Route("/getdishbyid/{id}")]
        public async Task<ActionResult<MenuDTO>> GetDishById(int id)
        {
            {
                var dish = await _menuService.GetDishByIdAsync(id);
                if (dish == null)
                {
                    return NotFound();
                }
                return Ok(dish);
            }
        }

        [Authorize(Policy = "RequireManagerOrAbove")]
        [HttpPost]
        [Route("/createmenu")]
        public async Task<ActionResult<MenuCreateDTO>> CreateMenu([FromBody] MenuCreateDTO menuCreateDTO)
        {
            try
            {
                var newDish = await _menuService.CreateMenuAsync(menuCreateDTO);
                newDish.Message = string.Format(ApiMessages.Success.Created, "Menu");
                return CreatedAtAction(nameof(GetDishById), new { id = newDish.Id }, newDish);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    error = ex.ErrorMessage, 
                    code = ex.ErrorCode
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the dish.");
            }
        }

        [Authorize(Policy = "RequireSuperAdmin")]
        [HttpDelete]
        [Route("/deletemenu/{id}")]
        public async Task<ActionResult> DeleteMenuDish(int id)
        {
            try
            {
                var deletedDish = await _menuService.DeleteMenuAsync(id);
                deletedDish.Message = string.Format(ApiMessages.Success.Deleted, "Menu");
                return Ok(deletedDish);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    error = ex.ErrorMessage,  
                    code = ex.ErrorCode
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the dish from menu.");
            }
        }

        [Authorize(Policy = "RequireManagerOrAbove")]
        [HttpPut]
        [Route("/updatemenu/{id}")]
        public async Task<ActionResult> UpdateMenu(int id, MenuUpdateDTO menuUpdateDTO)
        {
            try
            {
                var updatedMenu = await _menuService.UpdateMenuAsync(id, menuUpdateDTO);
                updatedMenu.Message = string.Format(ApiMessages.Success.Updated, "Menu");
                return Ok(updatedMenu);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    error = ex.ErrorMessage, 
                    code = ex.ErrorCode
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the menu.");
            }
        }



    }
}
