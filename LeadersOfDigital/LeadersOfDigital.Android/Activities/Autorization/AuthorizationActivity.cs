
using Android.App;
using Android.OS;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using ViewModel = LeadersOfDigital.ViewModels.Authorization.AuthorizationViewModel;

namespace LeadersOfDigital.Android.Activities.AuthorizationViewModel
{
    [MvxActivityPresentation]
    [Activity]
    public class AuthorizationActivity : MvxActivity<ViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.AuthorizationActivity);
        }
    }
}
