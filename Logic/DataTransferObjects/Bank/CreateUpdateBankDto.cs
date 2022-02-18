using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.DataTransferObjects.Bank
{
    public class CreateUpdateBankDto
    {
        public string Name { get; set; }
       
        public double AmountOfCash { get; set; }
        public int Rating { get; set; }

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
