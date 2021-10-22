using Android.App;
using Android.OS;
using LeadersOfDigital.ViewModels;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace LeadersOfDigital.Android.Activities
{
    [MvxActivityPresentation]
    [Activity(Label = "AuthorizationActivity")]
    public class AuthorizationActivity : MvxActivity<AuthorizationViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }
    }
}
