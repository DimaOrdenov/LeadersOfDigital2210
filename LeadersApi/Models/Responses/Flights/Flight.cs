using System;

namespace LeadersApi.Models.Responses.Flights
{
    public class Flight
    {
        public string origin { get; set; }

        public string destination { get; set; }

        public string origin_airport { get; set; }

        public string destination_airport { get; set; }

        public int price { get; set; }

        public int duration { get; set; }

        public string link { get; set; }

        public DateTime departure_at { get; set; }

        public DateTime return_at { get; set; }
    }
}