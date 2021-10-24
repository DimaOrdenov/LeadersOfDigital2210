using System;
using LeadersOfDigital.DataModels.Responses.Hotels;
using LeadersOfDigital.ViewModels.Setup;

namespace LeadersOfDigital.Definitions
{
    public class Trip
    {
        public DateTime StartsAt { get; set; }

        public DateTime EndsAt { get; set; }

        public TicketItemViewModel FlightFrom { get; set; }

        public TicketItemViewModel FlightTo { get; set; }

        public HotelsResponse Hotel { get; set; }
    }
}
