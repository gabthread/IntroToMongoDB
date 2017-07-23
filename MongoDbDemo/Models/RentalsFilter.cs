using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoDbDemo.Models
{
    public class RentalsFilter
    {
        public decimal? PriceLimit { get; set; }
        public int? MinimumRooms { get; set; }
    }
}