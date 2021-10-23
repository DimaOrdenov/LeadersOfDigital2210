using System.Collections.Generic;
using Business.Definitions.Models.GooglePlacesApi;

namespace Business.Definitions.Responses
{
    public class GooglePlacesResponse
    {
        public List<Place> Results { get; set; }
        
        // public string Status { get; set; }
    }
}
