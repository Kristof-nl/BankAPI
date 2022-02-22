using AutoMapper;
using Data.DataObjects;
using Data.Repository;
using Logic.DataTransferObjects.Bank;
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
    public interface IBankService
    {
        Task<BankAdminDto> GetById(int bankId);
        Task<List<BankAdminDto>> GetAll();
        Task<BankAdminDto> Create(CreateUpdateBankDto createUpdateBankDto);
        Task<BankAdminDto> Update(BankAdminDto updateBankDto);
        Task Delete(int id);
        Task<CustomerDto> AddCustomerToBank(int bankId, CustomerDto entity);
    }


    public class BankService : IBankService
    {
        private readonly IBankRepository _bankRepository;
        private readonly IMapper _mapper;

        public BankService(
            IBankRepository bankRepository,
            IMapper mapper)
            
        {
            _bankRepository = bankRepository;
            _mapper = mapper;
        }

        public async Task<BankAdminDto> GetById(int bankId)
        {
            var bankFromDb = await _bankRepository.GetById(bankId).ConfigureAwait(false);


            if (bankFromDb == null)
            {
                throw new NotFoundException("Bank not found");
            }

            return _mapper.Map<Bank, BankAdminDto>(bankFromDb);
        }

        public async Task<List<BankAdminDto>> GetAll()
        {
            var allBanksFromDb = await _bankRepository.GetAll().ToListAsync().ConfigureAwait(false); 
                
            return _mapper.Map<List<Bank>, List<BankAdminDto>>(allBanksFromDb);
        }


        //Create
        public async Task<BankAdminDto> Create(CreateUpdateBankDto createUpdateBankDto)
        {
            var newBank = _mapper.Map<Bank>(createUpdateBankDto);
            await _bankRepository.Create(newBank);

            return _mapper.Map<Bank, BankAdminDto>(newBank);
        }


        //Update
        public async Task<BankAdminDto> Update(BankAdminDto updateBankAdminDto)
        {
            var bankToUpdate = _mapper.Map<Bank>(updateBankAdminDto);

            var checkBankInDataBase = await _bankRepository.GetById(bankToUpdate.Id).ConfigureAwait(false);
            if (checkBankInDataBase != null)
            {
                await _bankRepository.Update(bankToUpdate);
                return _mapper.Map<Bank, BankAdminDto>(bankToUpdate);
            }

            return null;
        }

        public async Task Delete(int id)
        {
            await _bankRepository.Delete(id);
        }

        public async Task<CustomerDto> AddCustomerToBank(int bankId, CustomerDto customer)
        {
            var bankFromDb = await _bankRepository.GetById(bankId).ConfigureAwait(false);

            if (bankFromDb != null)
            {
                var customerToAdd = _mapper.Map<Customer>(customer);

                var bankCustomer = await _bankRepository.AddCustomerToBank(bankId, customerToAdd);
                return _mapper.Map<Customer, CustomerDto>(bankCustomer);

            }

            return null;

        }
    }

    

}
