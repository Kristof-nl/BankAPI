using Logic.DataTransferObjects.Bank;
using Logic.DataTransferObjects.BankAccount;
using Logic.DataTransferObjects.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.DataTransferObjects.Customer
{
    public class ShortCustomerDtoWithTransaction

    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ShortBankDto Bank { get; set; }
        public ICollection<VeryShortBankAccountDtoWithTransaction> BankAccounts { get; set; }

    }
}
