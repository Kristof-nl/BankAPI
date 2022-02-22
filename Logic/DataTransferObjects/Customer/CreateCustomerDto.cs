using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.DataTransferObjects.Customer
{
    public class CreateCustomerDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //Address
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }

        //ContacInfo
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
