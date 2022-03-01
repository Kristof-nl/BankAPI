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

            var bank = await _mainDbContext.Banks
                //.Include(a => a.Address)
                .Include(x => x.Customers)
                .Include(b => b.BankAccounts)
                .Include(c => c.BankTransactions)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == bankAccount.Bank.Id);

            //Loan from bank where customer have an account
            if (bank.Name == bankName)
            {
                //Loan can't be greather than 5% of bank cash amount
                if(amount > (bank.AmountOfCash * 0.05))
                {
                    return null;
                }

                //Make new bank transaction and add it to DB
                var bankTransaction = new BankTransaction()
                {
                    Type = "Loan",
                    TransactionDate = DateTime.Now,
                    From = bank.Name,
                    To = (bankAccount.Customer.FirstName + " " + bankAccount.Customer.LastName),
                    Ammount =  amount,
                    AmmountBefore = bank.AmountOfCash,
                    AammountAfter = bank.AmountOfCash - amount,
                    Bank = bank,
                };

                bank.AmountOfCash -= amount;
                

                //Create transaction and add it to bank account
                Transaction transaction = new Transaction()
                {
                    Name = $"Loan from{bank.Name} {DateTime.Now.ToShortTimeString()}",
                    Type = "Loan",
                    TransactionDate = DateTime.Now,
                    From = bank.Name,
                    To = (bankAccount.Customer.FirstName + " " + bankAccount.Customer.LastName),
                    Ammount = amount,
                    AmmountBefore = bankAccount.AccountBalance,
                    AammountAfter = bankAccount.AccountBalance + amount,
                    BankAccount = bankAccount,
                };


                bankAccount.AccountBalance += amount;

                await _mainDbContext.Transactions.AddAsync(transaction);

                _mainDbContext.ChangeTracker.Clear();
                //await _mainDbContext.SaveChangesAsync();

                await _mainDbContext.BankTransactions.AddAsync(bankTransaction);

                _mainDbContext.Banks.Update(bank);
                _mainDbContext.BankAccounts.Update(bankAccount);
                await _mainDbContext.SaveChangesAsync();

                return bankAccount;
            }

            return null;
        }


    }
}
