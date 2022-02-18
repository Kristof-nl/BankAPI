﻿using CrossCuttingConcerns.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataObjects
{
    public class Bank : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public double AmountOfCash { get; set; }
        public int Rating { get; set; }
        public ICollection<Customer> Customers { get; set; }
    }
}
