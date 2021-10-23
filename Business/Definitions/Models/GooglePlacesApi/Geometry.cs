using Newtonsoft.Json;

namespace Business.Definitions.Models.GooglePlacesApi
{
    public class Geometry
    {
        public Position Location { get; set; }

        [JsonProperty("location_type")]
        public string LocationType { get; set; }

        // public Viewport viewport { get; set; }
    }
}
