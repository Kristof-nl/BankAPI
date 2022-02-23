using Data.DataObjects;
using Logic.DataTransferObjects.Bank;
using Logic.DataTransferObjects.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.DataTransferObjects.BankAccount
{
    public class ShortBankAccountDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string AccountNumber { get; set; }
    }
}
