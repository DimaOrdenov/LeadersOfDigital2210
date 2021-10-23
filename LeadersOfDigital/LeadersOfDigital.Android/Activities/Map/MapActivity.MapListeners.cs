using Android.Gms.Maps.Model;
using LeadersOfDigital.Android.Helpers;

namespace LeadersOfDigital.Android.Activities.Map
{
    public partial class MapActivity
    {
        public void OnPolylineClick(Polyline polyline)
        {
        }

        public void OnMapLongClick(LatLng point)
        {
            ViewModel.SelectDestinationCommand.Execute(point.ToPosition());
        }

        public void OnMapClick(LatLng point)
        {
            ViewModel.SelectedDestination = null;
        }
    }
}
