using AutoMapper;
using CrossCuttingConcerns.PagingSorting;
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
        Task<List<ShortBankDto>> GetAll();
        Task<BankAdminDto> Create(CreateUpdateBankDto createUpdateBankDto);
        Task<BankAdminDto> Update(BankAdminDto updateBankDto);
        Task Delete(int id);
        Task<PaginatedList<ShortBankDto>> GetPagedList(
          int? pageNumber, string sortField, string sortOrder,
          int? pageSize);
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

        public async Task<List<ShortBankDto>> GetAll()
        {
            var allBanksFromDb = await _bankRepository.GetAll().ToListAsync().ConfigureAwait(false); 
                
            return _mapper.Map<List<Bank>, List<ShortBankDto>>(allBanksFromDb);
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

       public async Task<PaginatedList<ShortBankDto>> GetPagedList(
       int? pageNumber, string sortField, string sortOrder,
       int? pageSize)
        {
            PaginatedList<Bank> result =
                await _bankRepository.GetSortList(pageNumber, sortField, sortOrder, pageSize);
            return new PaginatedList<ShortBankDto>
            {
                CurrentPage = result.CurrentPage,
                From = result.From,
                PageSize = result.PageSize,
                To = result.To,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                Items = result.Items.Select(ua => new ShortBankDto
                {
                    Id = ua.Id,
                    Name = ua.Name
                }).ToList()
            };
        }
    } 

}
