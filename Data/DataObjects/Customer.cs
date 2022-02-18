using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataObjects
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public Bank Bank { get; set; }
        public ICollection<BankAccount> BankAccounts { get; set; }
    }
}
