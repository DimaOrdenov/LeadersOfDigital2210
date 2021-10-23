using System.Collections.Generic;
using Business.Definitions.Models;
using Business.Definitions.Models.GoogleDirectionsApi;
using LeadersOfDigital.Helpers;

namespace LeadersOfDigital.Android.ViewModels.Map
{
    public class RouteItemViewModel
    {
        public RouteItemViewModel(Route route)
        {
            Points = route.OverviewPolyline.DecodePolylineIntoPoints();
        }

        public IEnumerable<Position> Points { get; }
    }
}
