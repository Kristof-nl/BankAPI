using AutoMapper;
using CrossCuttingConcerns.PagingSorting;
using Data.DataObjects;
using Data.Repository;
using Logic.DataTransferObjects.Bank;
using Logic.DataTransferObjects.BankAccount;
using Logic.DataTransferObjects.Customer;
using Logic.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IOperationService
    {
        Task<BankAccountDto> AskForLoan(string bankName, int amount, BankAccountDto bankAccount);
    }


    public class OperationService : IOperationService
    {
        private readonly IOperationRepository _operationRepository;
        private readonly IMapper _mapper;

        public OperationService(
            IOperationRepository operationRepositoryy,
            IMapper mapper)

        {
            _operationRepository = operationRepositoryy;
            _mapper = mapper;
        }


        public async Task<BankAccountDto> AskForLoan(string bankName, int amount, BankAccountDto bankAccount)
        {
            BankAccount bankNameExist = await _operationRepository.CheckBankName(bankName);

            var bankAccountToRepo = _mapper.Map<BankAccount>(bankAccount);

            if (bankNameExist == null)
            {
                throw new NotFoundException("Bank name not found");
            }


            await _operationRepository.AskForLoan(bankAccountToRepo, bankName, amount);
            return _mapper.Map<BankAccount, BankAccountDto>(bankAccountToRepo);
        }
    }

}
