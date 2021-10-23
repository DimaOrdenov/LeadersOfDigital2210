using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadersApi.Models.Responses.Flights
{
    public class Flights
    {
        public IEnumerable<Flight> data { get; set; }
    }
}
