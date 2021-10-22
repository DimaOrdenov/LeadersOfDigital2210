using Android.App;
using Android.Gms.Maps;
using Android.OS;
using LeadersOfDigital.ViewModels;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace LeadersOfDigital.Android.Activities
{
    [MvxActivityPresentation]
    [Activity]
    public class MapActivity : MvxActivity<MapViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MapActivity);

            // _map = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map_view_map);
 
        }
    }
}

