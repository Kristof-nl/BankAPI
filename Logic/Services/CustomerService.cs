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
    public interface ICustomerService
    {
        Task<CustomerDto> GetById(int bankId);
        Task<List<CustomerDto>> GetAll();
        Task<CustomerDto> Create(CreateCustomerDto createUpdateBankDto);
        Task<CustomerDto> Update(CustomerDto updateCustomerDto);
        Task Delete(int id);
    }


    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(
            ICustomerRepository customerRepository,
            IMapper mapper)
            
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<CustomerDto> GetById(int bankId)
        {
            var customerFromDb = await _customerRepository.GetById(bankId).ConfigureAwait(false);


            if (customerFromDb == null)
            {
                throw new NotFoundException("Customer not found");
            }

            return _mapper.Map<Customer, CustomerDto>(customerFromDb);
        }

        public async Task<List<CustomerDto>> GetAll()
        {
            var allCustomersFromDb = await _customerRepository.GetAll().ToListAsync().ConfigureAwait(false); 
                
            return _mapper.Map<List<Customer>, List<CustomerDto>>(allCustomersFromDb);
        }


        //Create
        public async Task<CustomerDto> Create(CreateCustomerDto createUpdateBankDto)
        {
            var newCustomer = _mapper.Map<Customer>(createUpdateBankDto);
            await _customerRepository.Create(newCustomer);

            return _mapper.Map<Customer, CustomerDto>(newCustomer);
        }


        //Update
        public async Task<CustomerDto> Update(CustomerDto updateCustomerDto)
        {
            var customerToUpdate = _mapper.Map<Customer>(updateCustomerDto);

            var checkCustomerInDataBase = await _customerRepository.GetById(updateCustomerDto.Id).ConfigureAwait(false);
            if (checkCustomerInDataBase != null)
            {
                await _customerRepository.Update(customerToUpdate);
                return _mapper.Map<Customer, CustomerDto>(customerToUpdate);
            }

            return null;
        }

        public async Task Delete(int id)
        {
            await _customerRepository.Delete(id);
        }
    }

    
}
