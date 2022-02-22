using Data.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.DataTransferObjects.Customer
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public Data.DataObjects.Bank Bank { get; set; }
        public ICollection<BankAccount> BankAccounts { get; set; }
    }
}
