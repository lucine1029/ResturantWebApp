using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantAPI.Data.Repositories.IRepositories;
using RestaurantAPI.Models;

namespace RestaurantAPI.Data.Repositories
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly ApplicationDbContext _context;
        public CustomerRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            var customers = await _context.Customer.ToListAsync();
            return customers;
        }

        public async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            var customer = await _context.Customer.FirstOrDefaultAsync(t => t.Id == customerId);
            return customer;
        }

        public async Task<Customer> GetCustomerByPhoneNumberAsync(string phoneNumber)
        {
            var customer = await _context.Customer.FirstOrDefaultAsync(t => t.Phone == phoneNumber);
            return customer;
        }

        public async Task<int> AddCustomerAsync(Customer customer)
        {
            await _context.Customer.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer.Id;
        }
        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            var rowsAffected = await _context.Customer
                .Where(t => t.Id == customerId)
                .ExecuteDeleteAsync();
            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            _context.Customer.Update(customer);
            var result = await _context.SaveChangesAsync();
            return true;
        }
    }
}
