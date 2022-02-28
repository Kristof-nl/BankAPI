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

        Task<PaginatedList<BankAccount>> GetSortList(
           int? pageNumber,
           string sortField,
           string sortOrder,
           int? pageSize);

        Task<BankAccount> GetById(int id);
        Task<BankAccount> GetByIdWithTracking(int id);
        Task<BankAccount> Create(int bankId, BankAccount entity);
        Task Update(BankAccount entity);
        Task Delete(int id);
        Task<Customer> AddCustomerToAccount(int accountId, Customer entity);
        Task<IQueryable<BankAccount>> GetAllWithExtraFields();
        Task<BankAccount> AskForLoan(int bankAccountId);



    }

    public class BankAccountRepository : GenericRepository<BankAccount>, IBankAccountRepository
    {
        private readonly MainDbContext _mainDbContext;

        public BankAccountRepository(MainDbContext mainDbContext) : base(mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        private const int PageSize = 10;

        public override async Task<BankAccount> GetById(int id)
        {
            return await GetAll()
                .Include(c => c.Customer)
                .Include(b => b.Bank)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id)
                .ConfigureAwait(false);
        }

        public async Task<IQueryable<BankAccount>> GetAllWithExtraFields()
        {
            return _mainDbContext.BankAccounts.Include(c => c.Customer).Include(b => b.Bank).AsNoTracking();
        }


        //Create
        public async Task<BankAccount> Create(int bankId, BankAccount entity)
        {
            //Make random account number
            Random rnd = new();
            int first = rnd.Next(0000, 9999);
            int second = rnd.Next(0000, 9999);
            int third = rnd.Next(0000, 9999);

            entity.AccountNumber = $"0012 {first} 1678 {second} 8764 {third}";

            //Get bank to at it to entity
            var bank = await _mainDbContext.Banks
                .FirstOrDefaultAsync(x => x.Id == bankId)
                .ConfigureAwait(false);

            entity.Bank = bank;

            await _mainDbContext.BankAccounts.AddAsync(entity);


            //Make new bank transaction and add it to DB
            var bankTransaction = new BankTransaction()
            {
                Type = "Add new account",
                TransactionDate = DateTime.Now,
                Ammount = entity.AccountBalance,
                AmmountBefore = bank.AmountOfCash,
                AammountAfter = bank.AmountOfCash + entity.AccountBalance,
                Bank = bank,
            };

            //Create transaction and add it to bank account
            Transaction transaction = new()
            {
                Name = $"Account creation {DateTime.Now.ToString()}",
                Type = "Account creation",
                TransactionDate = DateTime.Now,
                Ammount = entity.AccountBalance,
                AmmountBefore = entity.AccountBalance,
                AammountAfter = entity.AccountBalance,
                BankAccount = entity,
            };

            await _mainDbContext.Transactions.AddAsync(transaction);

            await _mainDbContext.BankTransactions.AddAsync(bankTransaction);

            //Add cash to bank if start bank account is not 0
            bank.AmountOfCash += entity.AccountBalance;
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

        public async Task<Customer> AddCustomerToAccount(int accountId, Customer entity)
        {

            var bankAccount = await GetAll()
                .Include(x => x.Customer)
                .Include(b => b.Bank)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == accountId)
                .ConfigureAwait(false);


            //Add customer to bank
            bankAccount.Customer = entity;

            //Add bank and bank account to customer
            if(entity.Bank == null)
            {
                entity.BankAccounts.Add(bankAccount);
                entity.Bank = bankAccount.Bank;
            }

            _mainDbContext.Update(entity);
            await _mainDbContext.SaveChangesAsync();
            
            return entity;

        }

        public async Task<PaginatedList<BankAccount>> GetSortList(
          int? pageNumber,
          string sortField,
          string sortOrder,
          int? pageSize)
        {
            IQueryable<BankAccount> query = _mainDbContext.BankAccounts
                .Include(p => p.Transactions)
                .Include(p => p.Customer)
                .Include(x => x.Bank);

            return await PaginatedList<BankAccount>
               .CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize ?? PageSize, sortField ?? "Id", sortOrder ?? "ASC");
        }


        public async Task<BankAccount> AskForLoan(int bankAccountId)
        {
            var bankAccount = await GetAll()
                .Include(x => x.Customer)
                .Include(b => b.Bank)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == bankAccountId)
                .ConfigureAwait(false);

            var bank = await _mainDbContext.Banks.FirstOrDefaultAsync(x => x.Id == bankAccount.Bank.Id);

            return null;
        }
    }
}
