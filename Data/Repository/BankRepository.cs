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
    public interface IBankRepository
    {
        IQueryable<Bank> GetAll();

        Task<PaginatedList<Bank>> GetList(
            int? pageNumber,
            string sortField,
            string sortOrder,
            int? pageSize);

        Task<Bank> GetById(int id);
        Task<Bank> GetByIdWithTracking(int id);
        Task<Bank> Create(Bank entity);
        Task Update(Bank entity);
        Task Delete(int id);
        Task<Customer> AddCustomerToBank(int bankId, Customer entity);
            

    }

    public class BankRepository : GenericRepository<Bank>, IBankRepository
    {
        private readonly MainDbContext _mainDbContext;

        public BankRepository(MainDbContext mainDbContext) : base(mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public override async Task<Bank> GetById(int id)
        {
            return await GetAll()
                .Include(a => a.Address)
                .ThenInclude(a => a.ContactInfo)
                .Include(c => c.Customers)
                .FirstOrDefaultAsync(x => x.Id == id)
                .ConfigureAwait(false);
        }


        //Create
        public override async Task<Bank> Create(Bank entity)
        {

            await _mainDbContext.Banks.AddAsync(entity);
            await _mainDbContext.SaveChangesAsync();
            return entity;
        }


        //Update
        public override async Task<Bank> Update(Bank entity)
        {
            _mainDbContext.Update(entity);
            await _mainDbContext.SaveChangesAsync();
            return entity;
        }


        //Delete
        public override async Task Delete(int id)
        {
            var personToDelete = await _mainDbContext.Banks.FirstOrDefaultAsync(p => p.Id == id);

            _mainDbContext.Banks.Remove(personToDelete);
            await _mainDbContext.SaveChangesAsync();
        }

        public async Task<Customer> AddCustomerToBank(int bankId, Customer entity)
        {

            var bank = await GetAll()
                .Include(a => a.Address)
                .ThenInclude(a => a.ContactInfo)
                .Include(c => c.Customers)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == bankId)
                .ConfigureAwait(false);

            bank.Customers.Add(entity);
            _mainDbContext.Update(bank);
            await _mainDbContext.SaveChangesAsync();

            return entity;
        }

    }
}
