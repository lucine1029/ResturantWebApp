using RestaurantAPI.Models.DTOs.Customer;
using RestaurantAPI.Models.DTOs.Table;

namespace RestaurantAPI.Services.IServices
{
    public interface ICustomerService
    {
        Task<List<CustomerDTO>> GetAllCustomersAsync();
        Task<CustomerDTO> GetCustomerByIdAsync(int customerId);
        Task<CustomerDTO> GettCustomerByPhoneNumberAsync(string phoneNumber);
        Task<CustomerMessageDTO> CreateCustomerAsync(CustomerCreateDTO customerCreateDTO);
        Task<CustomerMessageDTO> UpdateCustomerAsync(int customerId, CustomerUpdateDTO customeUpdateDto);
        Task<CustomerMessageDTO> DeleteCustomerAsync(int customerId);
    }
}
