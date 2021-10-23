using System.Collections.Generic;
using Business.Definitions.Models.GoogleGeocodeApi;
using Business.Definitions.Models.GooglePlacesApi;
using Newtonsoft.Json;

namespace Business.Definitions.Responses
{
    public class GoogleGeocodeItemResponse
    {
        [JsonProperty("address_components")]
        public IEnumerable<AddressComponent> AddressComponents { get; set; }

        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        public Geometry Geometry { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("plus_code")]
        public PlusCode PlusCode { get; set; }

        public IEnumerable<string> Types { get; set; }
    }
}
