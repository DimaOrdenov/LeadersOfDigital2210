using Newtonsoft.Json;

namespace Business.Definitions.Models.GoogleDirectionsApi
{
    public class GeocodedWaypoint
    {
        [JsonProperty("geocoder_status")]
        public string GeocoderStatus { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        // public IList<string> Types { get; set; }
    }
}
