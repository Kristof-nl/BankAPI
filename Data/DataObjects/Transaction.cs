using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataObjects
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Type { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double ammount { get; set; }
        public double ammountBefore { get; set; }
        public double ammountAfter { get; set; }
        public BankAccount BankAccount { get; set; }

    }
}
