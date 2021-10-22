using System;
using Dal.Types;

namespace Dal.Entities
{
    public class Route
    {
        public int Id { get; set; }

        public TransportType Transport { get; set; }

        public DateTime StartsAt { get; set; }

        public DateTime EndsAt { get; set; }

        public int OriginId { get; set; }
        public Place Origin { get; set; }

        public int DestinationId { get; set; }
        public Place Destination { get; set; }
    }
}
