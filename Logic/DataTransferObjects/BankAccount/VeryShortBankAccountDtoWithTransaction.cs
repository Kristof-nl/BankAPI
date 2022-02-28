using Data.DataObjects;
using Logic.DataTransferObjects.Bank;
using Logic.DataTransferObjects.Customer;
using Logic.DataTransferObjects.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.DataTransferObjects.BankAccount
{
    public class VeryShortBankAccountDtoWithTransaction
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string AccountNumber { get; set; }
        public double AccountBalance { get; set; }
        public ICollection<ShortTransactionDto> Transactions { get; set; }
    }
}
