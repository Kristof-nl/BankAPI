using CrossCuttingConcerns.PagingSorting;
using Logic.DataTransferObjects.Bank;
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
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;
        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }


        [AllowAnonymous]
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var bank = await _bankService
                    .GetById(id)
                    .ConfigureAwait(false);

                return Ok(bank);
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
                var banks =
                    await _bankService
                    .GetAll()
                    .ConfigureAwait(false);

                return Ok(banks);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateUpdateBankDto createUpdateBankDto)
        {
            try
            {
                var newBank =
                    await _bankService
                        .Create(createUpdateBankDto)
                        .ConfigureAwait(false);

                return Ok(newBank);

            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] BankAdminDto updateBankDto)
        {
            try
            {
                var bankToUpdate = await _bankService.Update(updateBankDto).ConfigureAwait(true);
                if (bankToUpdate != null)
                {
                    return Ok(bankToUpdate);
                }
                return BadRequest("Bank doesn't exist in the database.");

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
                var bank = await _bankService.GetById(id).ConfigureAwait(false);

                if (bank == null)
                {
                    return BadRequest("Bank doesn't exist in the database.");
                }
                await _bankService.Delete(id).ConfigureAwait(true);
                return Ok("Bank has been deleted");
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("GetPagedList")]
        public async Task<ActionResult<PaginatedList<ShortBankDto>>> Get(
            int? pageNumber, string sortField, string sortOrder,
            int? pageSize)
        {
            var list = await _bankService.GetPagedList(pageNumber, sortField, sortOrder, pageSize);
            return list;
        }

    }
}

