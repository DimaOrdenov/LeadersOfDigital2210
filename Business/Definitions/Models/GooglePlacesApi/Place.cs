using System.Collections.Generic;
using Newtonsoft.Json;

namespace Business.Definitions.Models.GooglePlacesApi
{
    public class Place
    {
        // [JsonProperty("business_status")]
        // public string BusinessStatus { get; set; }

        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        public Geometry Geometry { get; set; }

        // public string icon { get; set; }
        // public string icon_background_color { get; set; }
        // public string icon_mask_base_uri { get; set; }

        public string Name { get; set; }

        // public OpeningHours opening_hours { get; set; }
        // public List<Photo> photos { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        // public PlusCode plus_code { get; set; }
        // public double rating { get; set; }
        // public string reference { get; set; }
        
        public List<string> Types { get; set; }
        
        // public int user_ratings_total { get; set; }
    }
}
