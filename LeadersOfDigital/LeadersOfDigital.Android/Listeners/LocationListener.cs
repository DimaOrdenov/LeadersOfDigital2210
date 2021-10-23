using System;
using Android.Locations;
using Android.OS;

namespace LeadersOfDigital.Android.Listeners
{
    public class LocationListener : Java.Lang.Object, ILocationListener
    {
        public event EventHandler<Location> LocationChanged;

        public void OnLocationChanged(Location location)
        {
            LocationChanged?.Invoke(this, location);
        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
        }
    }
}
