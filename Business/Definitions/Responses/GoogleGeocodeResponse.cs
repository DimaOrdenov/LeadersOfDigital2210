using System.Collections.Generic;
using Business.Definitions.Models.GoogleGeocodeApi;
using Newtonsoft.Json;

namespace Business.Definitions.Responses
{
    public class GoogleGeocodeResponse
    {
        [JsonProperty("plus_code")]
        public PlusCode PlusCode { get; set; }
        
        public IEnumerable<GoogleGeocodeItemResponse> Results { get; set; }
    }
}
