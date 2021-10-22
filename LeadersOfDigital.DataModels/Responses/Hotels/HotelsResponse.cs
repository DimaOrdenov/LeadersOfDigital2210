using System;
using System.Collections.Generic;
using System.Text;

namespace LeadersOfDigital.DataModels.Responses.Hotels
{
    public class HotelsResponse
    {
        public int HotelId { get; set; }

        public int Stars { get; set; }

        public double PriceAvg { get; set; }
        public string HotelName { get; set; }

        public double PriceFrom { get; set; }

        public float Lat { get; set; }

        public float Lon { get; set; }
    }
}
