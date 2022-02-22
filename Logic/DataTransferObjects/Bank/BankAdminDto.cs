using Data.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.DataTransferObjects.Bank
{
    public class BankAdminDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public double AmountOfCash { get; set; }
        public int Rating { get; set; }
        public ICollection<Data.DataObjects.Customer> Customers { get; set; }
    }
}
