using CrossCuttingConcerns.PagingSorting;
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
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;
        public BankAccountController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }


        [AllowAnonymous]
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var bankAccount = await _bankAccountService
                    .GetById(id)
                    .ConfigureAwait(false);

                return Ok(bankAccount);
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
                var bankAccounts =
                    await _bankAccountService
                    .GetAll()
                    .ConfigureAwait(false);

                return Ok(bankAccounts);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(int bankId, [FromBody] CreateBankAccountDto createBankAccountDto)
        {
            try
            {
                if(bankId == 0)
                {
                    return BadRequest("Please write a bank Id");
                }

                if (createBankAccountDto.AccountBalance < 0)
                {
                    return BadRequest("Account balance must be at least 0 ");
                }

                var newBankAccount =
                    await _bankAccountService
                        .Create(bankId, createBankAccountDto)
                        .ConfigureAwait(false);

                return Ok(newBankAccount);

            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] BankAccountDto updateBankAccountDto)
        {
            try
            {
                var bankToUpdate = await _bankAccountService.Update(updateBankAccountDto).ConfigureAwait(true);
                if (bankToUpdate != null)
                {
                    return Ok(bankToUpdate);
                }
                return BadRequest("Bank account doesn't exist in the database.");

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
                var bank = await _bankAccountService.GetById(id).ConfigureAwait(false);

                if (bank == null)
                {
                    return BadRequest("Bank account doesn't exist in the database.");
                }
                await _bankAccountService.Delete(id).ConfigureAwait(true);
                return Ok("Bank account has been deleted");
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("AddCustomerToAccount")]
        public async Task<IActionResult> AddCustomerToAccount(int accountId, CustomerDto customerDto)
        {
            var accountToAdd = await _bankAccountService.AddCustomerToAccount(accountId, customerDto);

            if (accountToAdd == null)
            {
                return BadRequest("Bank account doesn't exist in the database.");
            }
            return Ok("Customer was added to the bank account");
        }


        [AllowAnonymous]
        [HttpGet("GetPagedList")]
        public async Task<ActionResult<PaginatedList<ShortBankAccountDto>>> Get(
            int? pageNumber, string sortField, string sortOrder,
            int? pageSize)
        {
            var list = await _bankAccountService.GetPagedList(pageNumber, sortField, sortOrder, pageSize);
            return list;
        }
    }
}

