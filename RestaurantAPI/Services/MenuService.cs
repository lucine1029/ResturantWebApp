using RestaurantAPI.Data.Repositories;
using RestaurantAPI.Data.Repositories.IRepositories;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models.DTOs.Menu;
using RestaurantAPI.Models.DTOs.Table;
using RestaurantAPI.Services.IServices;
using System.ComponentModel.Design;
using RestaurantAPI.Constants;

namespace RestaurantAPI.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepo _menuRepo;
        public MenuService(IMenuRepo menuRepo)
        {
            _menuRepo = menuRepo;
        }

        public async Task<List<MenuMessageDTO>> GetAllDishesAsync()
        {
            var dishes = await _menuRepo.GetAllDishesAsync();
            var menusList = new List<MenuMessageDTO>();
            foreach (var d in dishes)
            {
                var menus = new MenuMessageDTO
                {
                    Id = d.Id,
                    DishName = d.DishName,
                    Description = d.Description,
                    IsAvailable = d.IsAvailable
                };
                menusList.Add(menus);
            }
            return menusList;
        }

        public async Task<MenuDTO> GetDishByIdAsync(int menuId)
        {

            var dish = await _menuRepo.GetDishByIdAsync(menuId);
            if (dish == null)
            {
                return null;
            }
            var menuDto = new MenuDTO
            {
                Id = dish.Id,
                DishName = dish.DishName,
                Description = dish.Description,
                Price = dish.Price,
                ImageUrl = dish.ImageUrl,
                IsAvailable = dish.IsAvailable,
                IsVegan = dish.IsVegan,
                HasNuts = dish.HasNuts,
                HasEgg = dish.HasEgg,
                HasDairy = dish.HasDairy,
                IsSpicy = dish.IsSpicy,
                IsGlutenFree = dish.IsGlutenFree
            };
            return menuDto;
        }

        public async Task<MenuMessageDTO> CreateMenuAsync(MenuCreateDTO menuCreateDTO)
        {
            //1. Input validation
            var existingDish = await _menuRepo.GetDishByDishNameAsync(menuCreateDTO.DishName);
            if (existingDish != null)
            {
                throw new ValidationException(
                    errorMessage: $"Dish name '{menuCreateDTO.DishName}' has already exist!",
                    errorCode: "Menu.DuplicateName"
                    );
            }
            //2.manually Map DTO to entity
            var newDish = new Models.Menu
            {
                DishName = menuCreateDTO.DishName,
                Description = menuCreateDTO.Description,
                Price = menuCreateDTO.Price,
                ImageUrl = menuCreateDTO.ImageUrl,
                IsAvailable = menuCreateDTO.IsAvailable,
                IsVegan = menuCreateDTO.IsVegan,
                HasNuts = menuCreateDTO.HasNuts,
                HasEgg = menuCreateDTO.HasEgg,
                HasDairy = menuCreateDTO.HasDairy,
                IsSpicy = menuCreateDTO.IsSpicy,
                IsGlutenFree = menuCreateDTO.IsGlutenFree
            };
            //3. Call the repository method to add the new dish
            var newDishId = await _menuRepo.AddMenuAsync(newDish);
            //Map the saved Entity back to a MenuDTO
            var newDishDTO = new MenuMessageDTO
            {
                Id = newDishId,
                DishName=newDish.DishName,
                Description = newDish.Description,
                IsAvailable=newDish.IsAvailable
    };
            return newDishDTO;
        }

        public async Task<MenuMessageDTO> DeleteMenuAsync(int menuId)
        {
            //1. Check if the dish exists
            var dish = await _menuRepo.GetDishByIdAsync(menuId);
            if (dish == null)
            {
                throw new ValidationException
                    (
                    errorMessage: $"Dish with Id '{menuId}' is not exist!",
                    errorCode: "Menu.NotFound"
                    );
            }
            //2. If all passed, then delete
            bool wasDeleted = await _menuRepo.DeleteMenuAsync(menuId);
            //4. check if repository/database deleted success or not
            if (!wasDeleted)
            {
                throw new Exception("Failed to delete the dish from Menu due to unexpected error.");
            }
            //5. Map back to the DTO
            return new MenuMessageDTO
            {
                Id = dish.Id,
                DishName = dish.DishName,
                Description = dish.Description,
                IsAvailable=dish.IsAvailable
            };
        }

        public async Task<MenuMessageDTO> UpdateMenuAsync(int menuId, MenuUpdateDTO menuUpdateDto)
        {
            //1.  get an existing dsish
            var existingDish = await _menuRepo.GetDishByIdAsync(menuId);
            if (existingDish == null)
            {
                throw new ValidationException
                    (
                    errorMessage: $"Dish with Id '{menuId}' is not exist!",
                    errorCode: "Menu.NotFound"
                    );
            }
            {
                //2.Modify
                existingDish.DishName = menuUpdateDto.DishName;
                existingDish.Description = menuUpdateDto.Description;
                existingDish.Price = menuUpdateDto.Price;
                existingDish.IsAvailable = menuUpdateDto.IsAvailable;
            }
            //3. Save
            var updatedTable = await _menuRepo.UpdateMenuAsync(existingDish);
            //5. Map and return 
            return new MenuMessageDTO
            {
                Id = existingDish.Id, //has not changed
                DishName= existingDish.DishName,
                Description = existingDish.Description,
                IsAvailable = existingDish.IsAvailable
            };
        }
    }
}
