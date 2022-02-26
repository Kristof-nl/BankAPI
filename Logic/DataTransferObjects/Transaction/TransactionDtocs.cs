using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.DataTransferObjects.Transaction
{
    public class TransactionDtocs
    {
        public string Name { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Type { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double Ammount { get; set; }
        public double AmmountBefore { get; set; }
        public double AammountAfter { get; set; }
        public Data.DataObjects.BankAccount BankAccount { get; set; }
    }
}
