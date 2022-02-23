using CrossCuttingConcerns.Generics;
using CrossCuttingConcerns.PagingSorting;
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

        Task<PaginatedList<Customer>> GetList(
            int? pageNumber,
            string sortField,
            string sortOrder,
            int? pageSize);

        Task<Customer> GetById(int id);
        Task<Customer> GetByIdWithTracking(int id);
        Task<Customer> Create(Customer entity);
        Task Update(Customer entity);
        Task Delete(int id);
        //Task<BankAccount> AddAccountToCustomer(int customerId, BankAccount entity);


    }

    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly MainDbContext _mainDbContext;

        public CustomerRepository(MainDbContext mainDbContext) : base(mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

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

        //public async Task<BankAccount> AddAccountToCustomer(int customerId, BankAccount entity)
        //{

        //    var customer = await GetAll()
        //        .Include(c => c.BankAccounts)
        //        .Include(a => a.Address)
        //        .ThenInclude(c => c.ContactInfo)
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(x => x.Id == customerId)
        //        .ConfigureAwait(false);




        //    customer.BankAccounts.Add(entity);
        //    _mainDbContext.Update(customer);
        //    await _mainDbContext.SaveChangesAsync();

        //    return entity;
        //}
    }

    
}
