using RestaurantAPI.Models;

namespace RestaurantAPI.Data.Repositories.IRepositories
{
    public interface ICustomerRepo
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int customerId);
        Task<Customer> GetCustomerByPhoneNumberAsync(string phoneNumber);
        Task<int> AddCustomerAsync(Customer customer);
        Task<bool> UpdateCustomerAsync(Customer customer);
        Task<bool> DeleteCustomerAsync(int customerId);
    }
}
