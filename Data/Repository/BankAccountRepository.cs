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
    public interface IBankAccountRepository
    {
        IQueryable<BankAccount> GetAll();

        Task<PaginatedList<BankAccount>> GetList(
            int? pageNumber,
            string sortField,
            string sortOrder,
            int? pageSize);

        Task<BankAccount> GetById(int id);
        Task<BankAccount> GetByIdWithTracking(int id);
        Task<BankAccount> Create(BankAccount entity);
        Task Update(BankAccount entity);
        Task Delete(int id);
            

    }

    public class BankAccountRepository : GenericRepository<BankAccount>, IBankAccountRepository
    {
        private readonly MainDbContext _mainDbContext;

        public BankAccountRepository(MainDbContext mainDbContext) : base(mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public override async Task<BankAccount> GetById(int id)
        {
            return await GetAll()
                .Include(c => c.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id)
                .ConfigureAwait(false);
        }


        //Create
        public override async Task<BankAccount> Create(BankAccount entity)
        {
            Random rnd = new();
            int first = rnd.Next(0000, 9999);
            int second = rnd.Next(0000, 9999);
            int third = rnd.Next(0000, 9999);

            entity.AccountNumber = $"0012 {first} 1678 {second} 8764 {third}";

            await _mainDbContext.BankAccounts.AddAsync(entity);
            await _mainDbContext.SaveChangesAsync();
            return entity;
        }


        //Update
        public override async Task<BankAccount> Update(BankAccount entity)
        {
            _mainDbContext.Update(entity);
            await _mainDbContext.SaveChangesAsync();
            return entity;
        }


        //Delete
        public override async Task Delete(int id)
        {
            var accountToDelete = await _mainDbContext.BankAccounts.FirstOrDefaultAsync(p => p.Id == id);

            _mainDbContext.BankAccounts.Remove(accountToDelete);
            await _mainDbContext.SaveChangesAsync();
        }

    }
}
