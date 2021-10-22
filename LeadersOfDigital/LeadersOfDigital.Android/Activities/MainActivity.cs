using Android.App;
using Android.OS;
using LeadersOfDigital.ViewModels;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace LeadersOfDigital.Android.Activities
{
    [MvxActivityPresentation]
    [Activity]
    public class MainActivity : MvxActivity<MainViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainActivity);
        }
    }
}
