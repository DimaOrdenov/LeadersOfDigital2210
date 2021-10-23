using System.Collections.Generic;
using Newtonsoft.Json;

namespace Business.Definitions.Models.GoogleGeocodeApi
{
    public class AddressComponent
    {
        [JsonProperty("long_name")]
        public string LongName { get; set; }
        
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
        
        public IEnumerable<string> Types { get; set; }
    }
}
