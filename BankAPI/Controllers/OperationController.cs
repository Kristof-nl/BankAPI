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
    public class OperationController : ControllerBase
    {
        private readonly IOperationService _operationService;
        public OperationController(IOperationService operationService)
        {
            _operationService = operationService;
        }


        [AllowAnonymous]
        [HttpPost("AskForLoan")]
        public async Task<IActionResult> AskForLoan(string bankName, int amount, [FromBody]BankAccountDto bankAccount)
        {
            try
            {
                
                var loan = await _operationService.AskForLoan(bankName, amount, bankAccount);
               
                if (loan != null)
                {
                    return Ok(loan);
                }

                return Ok("Sorry your ask for loan has been refused.");

            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }



    }
}

