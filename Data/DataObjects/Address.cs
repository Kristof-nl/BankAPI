using CrossCuttingConcerns.Generics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataObjects
{
    public class Address : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        [ForeignKey("ContactInfo")]
        public int ContactInfoId { get; set; }
        public ContactInfo ContactInfo { get; set; }
    }
}
