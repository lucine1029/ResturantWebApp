using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantAPI.Data.Repositories;
using RestaurantAPI.Data.Repositories.IRepositories;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using RestaurantAPI.Models.DTOs.Customer;
using RestaurantAPI.Models.DTOs.Table;
using RestaurantAPI.Services.IServices;
using RestaurantAPI.Constants;

namespace RestaurantAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepo _customerRepo;
        public CustomerService(ICustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
        }


        public async Task<List<CustomerDTO>> GetAllCustomersAsync()
        {
            var customers = await _customerRepo.GetAllCustomersAsync();
            var customerList = new List<CustomerDTO>();
            foreach (var c in customers)
            {
                var customerDto = new CustomerDTO
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName, 
                    Email = c.Email,
                    Phone = c.Phone
                };
                customerList.Add(customerDto);
            }
            return customerList;
        }

        public async Task<CustomerDTO> GetCustomerByIdAsync(int customerId)
        {
            var customer = await _customerRepo.GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                return null;
            }
            var customerDto = new CustomerDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone
            };
            return customerDto;
        }

        public async Task<CustomerDTO> GettCustomerByPhoneNumberAsync(string phoneNumber)
        {
            var customer = await _customerRepo.GetCustomerByPhoneNumberAsync(phoneNumber);
            if (customer == null)
            {
                return null;
            }
            var customerDto = new CustomerDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone
            };
            return customerDto; 
        }

        public async Task<CustomerMessageDTO> CreateCustomerAsync(CustomerCreateDTO customerCreateDTO)
        {
            //1. Input validation 
            var existingCustomer = await _customerRepo.GetCustomerByPhoneNumberAsync(customerCreateDTO.Phone);//I assume phone number is also unique
            if (existingCustomer != null)
            {
                throw new ValidationException(
                    errorMessage: $"Customer {customerCreateDTO.FirstName} {customerCreateDTO.LastName} has already exist!",
                    errorCode: "Customer.Duplicate"
                    );
            }
            //2.manually Map DTO to entity
            var newCustomer = new Models.Customer
            {
                FirstName = customerCreateDTO.FirstName,
                LastName = customerCreateDTO.LastName,
                Email = customerCreateDTO.Email,
                Phone = customerCreateDTO.Phone
            };
            //3. Call the repository method to add the customer
            var newCustomerId = await _customerRepo.AddCustomerAsync(newCustomer);
            //4. Map the saved Entity back to a CustomerDTO 
            return new CustomerMessageDTO
            {
                Id = newCustomer.Id,
                FirstName = newCustomer.FirstName,
                LastName = newCustomer.LastName
            };
        }

        public async Task<CustomerMessageDTO> DeleteCustomerAsync(int customerId)
        {
            //1. Check if the customer exists
            var customer = await _customerRepo.GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                throw new ValidationException
                    (
                    errorMessage: $"Customer Id '{customerId}' is not exist!",
                    errorCode: "Customer.NotFound"
                    );
            }
            //3. If all passed, then delete
            bool wasDeleted = await _customerRepo.DeleteCustomerAsync(customerId);
            //4. check if repository/database deleted success or not
            if (!wasDeleted)
            {
                throw new Exception("Failed to delete the customer due to unexpected error.");
            }
            //5. Map back to the DTO
            return new CustomerMessageDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };
        }

        public async Task<CustomerMessageDTO> UpdateCustomerAsync(int customerId, CustomerUpdateDTO customeUpdateDto)
        {
            //1.  get an existing customer
            var existingCustomer = await _customerRepo.GetCustomerByIdAsync(customerId);
            if (existingCustomer == null)
            {
                throw new ValidationException
                    (
                    errorMessage: $"Customer Id '{customerId}' is not exist!",
                    errorCode: "Customer.NotFound"
                    );
            }
            //3. Modify
            existingCustomer.Email = customeUpdateDto.Email;
            existingCustomer.Phone = customeUpdateDto.Phone;
            //4. Save
            var updatedTable = await _customerRepo.UpdateCustomerAsync(existingCustomer);
            //5. Map and return 
            return new CustomerMessageDTO
            {
                Id = existingCustomer.Id, //has not changed
                FirstName = existingCustomer.FirstName,
                LastName = existingCustomer.LastName
            };
        }
    }
}
