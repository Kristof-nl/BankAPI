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
    public interface IOperationRepository
    {
        IQueryable<BankAccount> GetAll();
        Task<BankAccount> GetById(int id);
        Task<BankAccount> AskForLoan(BankAccount bankAccount, string bankName, int amount);
        Task<BankAccount> CheckBankName(string bankName);
    }

    public class OperationRepository : GenericRepository<BankAccount>, IOperationRepository
    {
        private readonly MainDbContext _mainDbContext;

        public OperationRepository(MainDbContext mainDbContext) : base(mainDbContext)
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


        public async Task<BankAccount> CheckBankName(string bankName)
        {
            return await GetAll()
                .Include(c => c.Customer)
                .Include(b => b.Bank)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Bank.Name == bankName)
                .ConfigureAwait(false);
            
        }


        public async Task<BankAccount> AskForLoan(BankAccount bankAccount, string bankName, int amount)
        {

            var bank = await _mainDbContext.Banks.FirstOrDefaultAsync(x => x.Id == bankAccount.Bank.Id);

            //Loan from bank where customer have an account
            if (bank.Name == bankName)
            {

            }

            return null;
        }


    }
}
