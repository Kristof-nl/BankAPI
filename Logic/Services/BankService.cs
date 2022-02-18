using AutoMapper;
using Data.DataObjects;
using Logic.DataTransferObjects.Bank;
using Logic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IBankService
    {
        Task<BankAdminDto> GetById(int personId);
        Task<List<BankAdminDto>> GetAll();
        Task<BankAdminDto> Create(CreateUpdateBankDto createUpdatePersonDto);
        Task<BankAdminDto> Update(BankAdminDto updatePersonDto);
        Task Delete(int id);
    }


    public class BankService : IBankService
    {
        private readonly IBankRepository _bankRepository;
        private readonly IMapper _mapper;

        public PersonService(
            IBankRepository bankRepository,
            IMapper mapper
            )
        {
            _bankRepository = bankRepository;
            _mapper = mapper;
        }

        public async Task<BankAdminDto> GetById(int personId)
        {
            var bankFromDb = await _bankRepository.GetById(personId).ConfigureAwait(false);


            if (bankFromDb == null)
            {
                throw new NotFoundException("Person not found");
            }

            return _mapper.Map<Bank, BankAdminDto>(bankFromDb);
        }

        public async Task<List<BankAdminDto>> GetAll()
        {
            var allPersonsFromDb = await _bankRepository
                .GetAll()
                .Include(p => p.Address)
                .ToListAsync().ConfigureAwait(false);
            return _mapper.Map<List<Bank>, List<BankAdminDto>>(allPersonsFromDb);
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
    }

    
}
