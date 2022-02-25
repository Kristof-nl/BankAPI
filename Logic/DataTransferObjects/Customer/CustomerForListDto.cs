using Data.DataObjects;
using Logic.DataTransferObjects.Bank;
using Logic.DataTransferObjects.BankAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.DataTransferObjects.Customer
{
    public class CustomerForListDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public ShortBankDto Bank { get; set; }
        public ICollection<VeryShortBankAccountDto> BankAccounts { get; set; }
    }
}
