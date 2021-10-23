using Android.Gms.Maps.Model;
using Android.Locations;
using Business.Definitions.Models;

namespace LeadersOfDigital.Android.Helpers
{
    public static class MapExtensions
    {
        public static LatLng ToLatLng(this Position position) =>
            new (position.Lat, position.Lng);
        
        public static LatLng ToLatLng(this Location location) =>
            new (location.Latitude, location.Longitude);

        public static Position ToPosition(this LatLng latLng) =>
            new (latLng.Latitude, latLng.Longitude);
        
        public static Position ToPosition(this Location location) =>
            new (location.Latitude, location.Longitude);
    }
}
