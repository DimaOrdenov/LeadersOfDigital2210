using System;
using System.Collections.Generic;
using Business.Definitions.Models;

namespace Business.Definitions.Requests
{
    public class GoogleApiDirectionsRequest
    {
        public GoogleApiDirectionsRequest(Func<Position> origin, Func<Position> destination, string travelMode = "driving", bool isProvideAlternatives = true)
        {
            Origin = origin;
            Destination = destination;
            TravelMode = travelMode;
            IsProvideAlternatives = isProvideAlternatives;
        }

        /// <summary>
        /// driving, walking, transit
        /// </summary>
        public string TravelMode { get; }

        public Func<Position> Origin { get; }

        public Func<Position> Destination { get; }

        public IEnumerable<Position> Waypoints { get; set; }
        
        public bool IsProvideAlternatives { get; }
    }
}
