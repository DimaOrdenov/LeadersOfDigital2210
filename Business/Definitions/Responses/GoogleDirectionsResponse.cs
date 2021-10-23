using System.Collections.Generic;
using Business.Definitions.Models.GoogleDirectionsApi;

namespace Business.Definitions.Responses
{
    public class GoogleDirectionsResponse
    {
        // [JsonProperty("geocoded_waypoints")]
        // public IList<GeocodedWaypoint> GeocodedWaypoints { get; set; }

        public IEnumerable<Route> Routes { get; set; }

        // public string Status { get; set; }
    }
}
