using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossCuttingConserns.Filters
{
    public class CustomerFilter
    {
        public string? Country { get; set; } = null;
        public string? BankName { get; set; } = null;
        public string? AccountType { get; set; } = null;
        public double? AccountBalanceFrom { get; set; } 
        public double? AccountBalanceTo { get; set; }
        public DateTime? CreationDateFrom { get; set; }
        public DateTime? CreationDateTo { get; set; }
        public int? AgeFrom { get; set; }
        public int? AgeTo { get; set; }


    }
}
