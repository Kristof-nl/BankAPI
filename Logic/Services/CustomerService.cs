using AutoMapper;
using CrossCuttingConcerns.PagingSorting;
using CrossCuttingConserns.Filters;
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
    public interface ICustomerService
    {
        Task<CustomerDto> GetById(int customerId);
        Task<List<CustomerDto>> GetAll();
        Task<CustomerDto> Create(CreateCustomerDto createUpdateBankDto);
        Task<CustomerDto> Update(CustomerDto updateCustomerDto);
        Task Delete(int id);
        Task<PaginatedList<CustomerForListDto>> GetPagedList(
          int? pageNumber, string sortField, string sortOrder,
          int? pageSize);
        Task<PaginatedList<CustomerForListDto>> SearchWithPaging(
        int? pageNumber, string sortField, string sortOrder,
        int? pageSize, string searchField, string searchName);
        Task<PaginatedList<CustomerForListDto>> Filter(CustomerFilter profileFilterDto, int? pageNumber, string sortField, string sortOrder,
           int? pageSize);
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

        public async Task<CustomerDto> GetById(int customerId)
        {
            var customerFromDb = await _customerRepository.GetById(customerId).ConfigureAwait(false);


            if (customerFromDb == null)
            {
                throw new NotFoundException("Customer not found");
            }

            return _mapper.Map<Customer, CustomerDto>(customerFromDb);
        }

        public async Task<List<CustomerDto>> GetAll()
        {
            var allCustomersFromDb = await _customerRepository.GetAll()
                .Include(b => b.Bank)
                .Include(ba => ba.BankAccounts)
                .Include(a => a.Address)
                .ThenInclude(a => a.ContactInfo)
                .ToListAsync()
                .ConfigureAwait(false); 
                
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


       public async Task<PaginatedList<CustomerForListDto>> GetPagedList(
       int? pageNumber, string sortField, string sortOrder,
       int? pageSize)
        {
            PaginatedList<Customer> result =
                await _customerRepository.GetSortList(pageNumber, sortField, sortOrder, pageSize);
            return new PaginatedList<CustomerForListDto>
            {
                CurrentPage = result.CurrentPage,
                From = result.From,
                PageSize = result.PageSize,
                To = result.To,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                Items = result.Items.Select(ua => new CustomerForListDto
                {
                    Id = ua.Id,
                    FirstName = ua.FirstName,
                    LastName = ua.LastName,
                    Address = ua.Address,
                    BankAccounts = _mapper.Map<List<VeryShortBankAccountDto>>(ua.BankAccounts),
                    
                    Bank = _mapper.Map<ShortBankDto>(ua.Bank),
                }).ToList()
            };
        }

        public async Task<PaginatedList<CustomerForListDto>> SearchWithPaging(
       int? pageNumber, string sortField, string sortOrder,
       int? pageSize, string searchField, string searchName)
        {
            PaginatedList<Customer> result =
                await _customerRepository.SearchWithPaging(pageNumber, sortField, sortOrder, pageSize, searchField, searchName);
            return new PaginatedList<CustomerForListDto>
            {
                CurrentPage = result.CurrentPage,
                From = result.From,
                PageSize = result.PageSize,
                To = result.To,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                Items = result.Items.Select(ua => new CustomerForListDto
                {
                    Id = ua.Id,
                    FirstName = ua.FirstName,
                    LastName = ua.LastName,
                    Address = ua.Address,
                    BankAccounts = _mapper.Map<List<VeryShortBankAccountDto>>(ua.BankAccounts),

                    Bank = _mapper.Map<ShortBankDto>(ua.Bank),
                }).ToList()
            };
        }

        public async Task<PaginatedList<CustomerForListDto>> Filter(CustomerFilter profileFilterDto, int? pageNumber, string sortField, string sortOrder,
           int? pageSize)
        {
            PaginatedList<Customer> result =
                await _customerRepository.Filter(profileFilterDto, pageNumber, sortField, sortOrder, pageSize);
            return new PaginatedList<CustomerForListDto>
            {
                CurrentPage = result.CurrentPage,
                From = result.From,
                PageSize = result.PageSize,
                To = result.To,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                Items = result.Items.Select(ua => new CustomerForListDto
                {
                    Id = ua.Id,
                    FirstName = ua.FirstName,
                    LastName = ua.LastName,
                    Address = ua.Address,
                    BankAccounts = _mapper.Map<List<VeryShortBankAccountDto>>(ua.BankAccounts),

                    Bank = _mapper.Map<ShortBankDto>(ua.Bank),
                }).ToList()
            };
        }
    }

    
}
