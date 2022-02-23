using AutoMapper;
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
    public interface IBankAccountService
    {
        Task<BankAccountDto> GetById(int bankId);
        Task<List<ShortBankAccountDto>> GetAll();
        Task<BankAccountDto> Create(int bankId, CreateBankAccountDto createUpdateBankDto);
        Task<BankAccountDto> Update(BankAccountDto updateBankAccountDto);
        Task Delete(int id);
        Task<CustomerDto> AddCustomerToAccount(int accountId, CustomerDto customerDto);
    }


    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IMapper _mapper;

        public BankAccountService(
            IBankAccountRepository bankAccountRepository,
            IMapper mapper)

        {
            _bankAccountRepository = bankAccountRepository;
            _mapper = mapper;
        }

        public async Task<BankAccountDto> GetById(int bankAccountId)
        {
            var bankAccountFromDb = await _bankAccountRepository.GetById(bankAccountId).ConfigureAwait(false);


            if (bankAccountFromDb == null)
            {
                throw new NotFoundException("Bank Account not found");
            }

            return _mapper.Map<BankAccount, BankAccountDto>(bankAccountFromDb);
        }

        public async Task<List<ShortBankAccountDto>> GetAll()
        {
            var allBankAccountsFromDb = await _bankAccountRepository.GetAllWithExtraFields().ConfigureAwait(false);

            return _mapper.Map<List<ShortBankAccountDto>>(allBankAccountsFromDb);
        }


        //Create
        public async Task<BankAccountDto> Create(int bankId, CreateBankAccountDto createBankAccountDto)
        {
            var newBankAccount = _mapper.Map<BankAccount>(createBankAccountDto);
            await _bankAccountRepository.Create(bankId, newBankAccount);

            return _mapper.Map<BankAccount, BankAccountDto>(newBankAccount);
        }


        //Update
        public async Task<BankAccountDto> Update(BankAccountDto updateBankAccountDto)
        {
            var bankAccountToUpdate = _mapper.Map<BankAccount>(updateBankAccountDto);

            var checkBankAccountInDataBase = await _bankAccountRepository.GetById(bankAccountToUpdate.Id).ConfigureAwait(false);
            if (checkBankAccountInDataBase != null)
            {
                await _bankAccountRepository.Update(bankAccountToUpdate);
                return _mapper.Map<BankAccount, BankAccountDto>(bankAccountToUpdate);
            }

            return null;
        }

        public async Task Delete(int id)
        {
            await _bankAccountRepository.Delete(id);
        }

        public async Task<CustomerDto> AddCustomerToAccount(int accountId, CustomerDto customerDto)
        {
            var accountFromDb = await _bankAccountRepository.GetById(accountId).ConfigureAwait(false);

            if (accountFromDb != null)
            {
                var customerToAdd = _mapper.Map<Customer>(customerDto);

                var customer = await _bankAccountRepository.AddCustomerToAccount(accountId, customerToAdd);
                return _mapper.Map<Customer, CustomerDto>(customer);

            }

            return null;

        }
    }

}
