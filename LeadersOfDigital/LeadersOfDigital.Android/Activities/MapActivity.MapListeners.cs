using Android.Gms.Maps.Model;
using Business.Definitions.Models;

namespace LeadersOfDigital.Android.Activities
{
    public partial class MapActivity
    {
        public void OnPolylineClick(Polyline polyline)
        {
        }

        public void OnMapLongClick(LatLng point)
        {
            ViewModel.SelectDestinationCommand.Execute(new Position(point.Latitude, point.Longitude));
        }

        public void OnMapClick(LatLng point)
        {
            ViewModel.SelectedDestination = null;
        }
    }
}
