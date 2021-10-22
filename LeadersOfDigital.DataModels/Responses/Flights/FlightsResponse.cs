using System;
using System.Collections.Generic;
using System.Text;

namespace LeadersOfDigital.DataModels.Responses.Flights
{
    public class FlightsResponse
    {

        public string Origin { get; set; }

        public string Destination { get; set; }

        public string Origin_airport { get; set; }

        public string Destination_airport { get; set; }

        public int Price { get; set; }

        public int Duration { get; set; }

        public string Link { get; set; }

        public DateTime Departure_at { get; set; }

        public DateTime Return_at { get; set; }
    }
}
