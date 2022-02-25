using CrossCuttingConcerns.Generics;
using CrossCuttingConcerns.PagingSorting;
using CrossCuttingConserns.Filters;
using Data.DataObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> GetAll();

        Task<PaginatedList<Customer>> GetSortList(
           int? pageNumber,
           string sortField,
           string sortOrder,
           int? pageSize);


        Task<PaginatedList<Customer>> SearchWithPaging(
           int? pageNumber,
           string sortField,
           string sortOrder,
           int? pageSize,
           string searchField,
           string searchName
           );

        Task<Customer> GetById(int id);
        Task<Customer> GetByIdWithTracking(int id);
        Task<Customer> Create(Customer entity);
        Task Update(Customer entity);
        Task Delete(int id);

        Task<PaginatedList<Customer>> Filter(CustomerFilter profileFilterDto, int? pageNumber, string sortField, string sortOrder,
            int? pageSize);

    }

    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly MainDbContext _mainDbContext;

        public CustomerRepository(MainDbContext mainDbContext) : base(mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        private const int PageSize = 10;

        public override async Task<Customer> GetById(int id)
        {
            return await GetAll()
                .Include(a => a.Address)
                .ThenInclude(a => a.ContactInfo)
                .Include(c => c.BankAccounts)
                .Include(c => c.Bank)
                .FirstOrDefaultAsync(x => x.Id == id)
                .ConfigureAwait(false); 
        }


        //Create
        public override async Task<Customer> Create(Customer entity)
        {

            await _mainDbContext.Customers.AddAsync(entity);
            await _mainDbContext.SaveChangesAsync();
            return entity;
        }


        //Update
        public override async Task<Customer> Update(Customer entity)
        {
            _mainDbContext.Update(entity);
            await _mainDbContext.SaveChangesAsync();
            return entity;
        }


        //Delete
        public override async Task Delete(int id)
        {
            var personToDelete = await _mainDbContext.Customers.FirstOrDefaultAsync(p => p.Id == id);

            _mainDbContext.Customers.Remove(personToDelete);
            await _mainDbContext.SaveChangesAsync();
        }

        public async Task<PaginatedList<Customer>> GetSortList(
          int? pageNumber,
          string sortField,
          string sortOrder,
          int? pageSize)
        {
            IQueryable<Customer> query = _mainDbContext.Customers
                .Include(p => p.BankAccounts)
                .Include(a => a.Address)
                .ThenInclude(a => a.ContactInfo)
                .Include(x => x.Bank);

            return await PaginatedList<Customer>
               .CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize ?? PageSize, sortField ?? "Id", sortOrder ?? "ASC");
        }


        public async Task<PaginatedList<Customer>> SearchWithPaging(
           int? pageNumber,
           string sortField,
           string sortOrder,
           int? pageSize,
           string searchField,
           string searchName
           )
        {
            searchField = searchField.Trim();
            searchName = searchName.Trim();
            IQueryable<Customer> query = _mainDbContext.Customers
                .Include(p => p.BankAccounts)
                .Include(a => a.Address)
                .ThenInclude(a => a.ContactInfo)
                .Include(x => x.Bank);

            switch (searchField)
            {
                case "LastName":
                    query = query.Where(e => e.LastName.Contains(searchName));
                    break;
                case "FirstName":
                    query = query.Where(e => e.FirstName.Contains(searchName));
                    break;
                case "Country":
                    query = query.Where(e => e.Address.Country.Contains(searchName));
                    break;
                case "City":
                    query = query.Where(e => e.Address.City.Contains(searchName));
                    break;
                case "BankName":
                    query = query.Where(e => e.Bank.Name.Contains(searchName));
                    break;
                default:
                    query = null;
                    break;
            }

            return await PaginatedList<Customer>
               .CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize ?? PageSize, sortField ?? "Id", sortOrder ?? "ASC");

        }


        public async Task<PaginatedList<Customer>> Filter(CustomerFilter filter, int? pageNumber, string sortField, string sortOrder,
            int? pageSize)
        {
            IQueryable<Customer> query = _mainDbContext.Customers
                .Include(p => p.BankAccounts)
                .Include(a => a.Address)
                .ThenInclude(a => a.ContactInfo)
                .Include(x => x.Bank);

            //Country
            if (filter.Country != null)
            {
                query = query.Where(p => p.Address.Country == filter.Country);
            }

            //Bank name
            if (filter.BankName != null)
            {
                query = query.Where(p => p.Bank.Name == filter.BankName);
            }

            //Type of account
            if (filter.AccountType != null)
            {
                query = query.Distinct().Where(e => e.BankAccounts.Any(x => x.Type.Contains(filter.AccountType)));
            }

            //Account balance
            if (filter.AccountBalanceFrom != null && filter.AccountBalanceTo != null )
            {
                query = query.Distinct().Where(e => e.BankAccounts.Any(x => x.AccountBalance >= filter.AccountBalanceFrom));
                query = query.Distinct().Where(e => e.BankAccounts.Any(x => x.AccountBalance <= filter.AccountBalanceTo));
            }

            //Account creation time
            if (filter.CreationDateFrom != null && filter.CreationDateTo == null)
            {
                filter.CreationDateTo = DateTime.Now;
            }
            if (filter.CreationDateFrom == null && filter.CreationDateTo != null)
            {
                filter.CreationDateTo = DateTime.Today.AddYears(-10);
            }
            if (filter.CreationDateFrom != null && filter.CreationDateTo != null)
            {
                query = query.Distinct().Where(e => e.BankAccounts.Any(x => x.CreationDate >= filter.CreationDateFrom));
                query = query.Distinct().Where(e => e.BankAccounts.Any(x => x.CreationDate <= filter.CreationDateTo));
            }

            //Age between

            if (filter.AgeFrom != null && filter.AgeTo == null)
            {
                filter.AgeTo = 100;
            }
            if (filter.AgeFrom == null && filter.AgeTo != null)
            {
                filter.AgeFrom = 0;
            }
            if (filter.AgeFrom != null && filter.AgeTo != null)
            {
                var minusAgeTo = filter.AgeTo * -1;
                var minusAgeFrom = filter.AgeFrom * -1;

                var ageTo = DateTime.Now.AddYears((int)minusAgeTo);
                var ageFrom = DateTime.Now.AddYears((int)minusAgeFrom);

                query = query.Where(e => e.DateOfBirth >= ageTo);
                query = query.Where(e => e.DateOfBirth <= ageFrom);
            }

            return await PaginatedList<Customer>
              .CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize ?? PageSize, sortField ?? "Id", sortOrder ?? "ASC");

        }

    }

}
