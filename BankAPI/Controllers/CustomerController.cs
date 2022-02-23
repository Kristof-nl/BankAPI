using Logic.DataTransferObjects.Bank;
using Logic.DataTransferObjects.BankAccount;
using Logic.DataTransferObjects.Customer;
using Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }


        [AllowAnonymous]
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var customer = await _customerService
                    .GetById(id)
                    .ConfigureAwait(false);

                return Ok(customer);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var customers =
                    await _customerService
                    .GetAll()
                    .ConfigureAwait(false);

                return Ok(customers);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto createUpdateBankDto)
        {
            try
            {
                var newCustomer =
                    await _customerService
                        .Create(createUpdateBankDto)
                        .ConfigureAwait(false);

                return Ok(newCustomer);

            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CustomerDto updateCustomerDto)
        {
            try
            {
                var customerToUpdate = await _customerService.Update(updateCustomerDto).ConfigureAwait(true);
                if (customerToUpdate != null)
                {
                    return Ok(customerToUpdate);
                }
                return BadRequest("Customer doesn't exist in the database.");

            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var customer = await _customerService.GetById(id).ConfigureAwait(false);

                if (customer == null)
                {
                    return BadRequest("Customer doesn't exist in the database.");
                }
                await _customerService.Delete(id).ConfigureAwait(true);
                return Ok("Customer has been deleted");
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        
        //[AllowAnonymous]
        //[HttpPost("AddAccountToCustomer")]
        //public async Task<IActionResult> AddAccountToCustomer(int customerId, BankAccountDto bankAccountDto)
        //{
        //    var accountToAdd = await _customerService.AddAccountToCustomer(customerId, bankAccountDto);

        //    if (accountToAdd == null)
        //    {
        //        return BadRequest("Bank account doesn't exist in the database.");
        //    }
        //    return Ok("Customer was added to the bank account");
        //}
    }
}

