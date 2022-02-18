using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataObjects
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime CreationDate { get; set; }
        public string Owner { get; set; }
        public double AccountBalance { get; set; }
        public Customer Customer { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
