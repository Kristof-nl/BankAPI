using Data.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.DataTransferObjects.BankAccount
{
    public class BankAccountDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string AccountNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public double AccountBalance { get; set; }

        public Data.DataObjects.Customer Customer { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
