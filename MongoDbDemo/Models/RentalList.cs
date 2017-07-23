using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoDbDemo.Models
{
    public class RentalList
    {
        public IEnumerable<Rental> Rentals { get; set; }
        public RentalsFilter Filters { get; set; }
    }
}