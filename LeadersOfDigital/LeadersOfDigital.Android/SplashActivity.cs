using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace LeadersOfDigital.Android
{
    [MvxActivityPresentation]
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        NoHistory = true,
        Theme = "@style/MainTheme",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : MvxSplashScreenActivity<Setup, App>
    {
        public SplashActivity()
            : base(Resource.Layout.SplashScreen)
        {
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Essentials.Platform.Init(this, bundle);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
