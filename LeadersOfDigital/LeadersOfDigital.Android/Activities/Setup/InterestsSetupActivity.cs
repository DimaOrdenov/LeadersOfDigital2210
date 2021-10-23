using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using LeadersOfDigital.ViewModels.Setup;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace LeadersOfDigital.Android.Activities.Setup
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class InterestsSetupActivity : MvxActivity<InterestsSetupViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.InterestsSetupActivity);
        }
    }
}

