using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Constants;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models.DTOs.Customer;
using RestaurantAPI.Models.DTOs.Table;
using RestaurantAPI.Services;
using RestaurantAPI.Services.IServices;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Authorize(Policy = "RequireStaffOrAbove")]
        [HttpGet]
        [Route("/getallcustomers")]
        public async Task<ActionResult<List<CustomerDTO>>> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [Authorize(Policy = "RequireStaffOrAbove")]
        [HttpGet]
        [Route("/getcustomerbyid/{id}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomerById(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [Authorize(Policy = "RequireStaffOrAbove")]
        [HttpGet]
        [Route("/getcustomerbyphonenumber/{phoneNumber}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomerByPhoneNumber(string phoneNumber)
        {
            var customerByPhoneNumber = await _customerService.GettCustomerByPhoneNumberAsync(phoneNumber);
            if (customerByPhoneNumber == null)
            {
                return NotFound();
            }
            return Ok(customerByPhoneNumber);
        }

        [Authorize(Policy = "RequireStaffOrAbove")]
        [HttpPost]
        [Route("/createcustomer")]
        public async Task<ActionResult<CustomerCreateDTO>> CreateCustomer([FromBody] CustomerCreateDTO customerCreateDTO)
        {
            try
            {
                var newCustomer = await _customerService.CreateCustomerAsync(customerCreateDTO);
                newCustomer.Message = string.Format(ApiMessages.Success.Created, "Customer");  //the constant success message for CRUD
                return CreatedAtAction(nameof(GetCustomerById), new { id = newCustomer.Id }, newCustomer);

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
                return StatusCode(500, "An error occurred while creating the customer.");
            }
        }

        [Authorize(Policy = "RequireSuperAdmin")]
        [HttpDelete]
        [Route("/deletecustomer/{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            try
            {
                var deletedCustomer = await _customerService.DeleteCustomerAsync(id);
                deletedCustomer.Message = string.Format(ApiMessages.Success.Deleted, "Customer");
                return Ok(deletedCustomer);
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
                return StatusCode(500, "An error occurred while deleting the customer.");
            }
        }

        [Authorize(Policy = "RequireManagerOrAbove")]
        [HttpPut]
        [Route("/updatecustomer/{id}")]
        public async Task<ActionResult> UpdateCustomer(int id, CustomerUpdateDTO customerUpdateDTO)
        {
            try
            {
                var updatedCustomer = await _customerService.UpdateCustomerAsync(id, customerUpdateDTO);
                updatedCustomer.Message = string.Format(ApiMessages.Success.Updated, "Customer");
                return Ok(updatedCustomer);
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
                return StatusCode(500, "An error occurred while updating the customer.");
            }
        }
    }
}
