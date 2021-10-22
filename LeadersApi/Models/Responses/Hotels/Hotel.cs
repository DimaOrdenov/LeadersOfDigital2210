using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadersApi.Models.Responses.Hotels
{
    public class Hotel
    {
        public int hotelId { get; set; }

        public int stars { get; set; }

        public double priceAvg { get; set; }
        public string hotelName { get; set; }

        public double priceFrom { get; set; }

        public Location location { get; set; }

       
    }
}
