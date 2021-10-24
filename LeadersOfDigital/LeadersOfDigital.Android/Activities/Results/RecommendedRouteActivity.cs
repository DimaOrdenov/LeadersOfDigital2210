using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using LeadersOfDigital.ViewModels.Results;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace LeadersOfDigital.Android.Activities.Results
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class RecommendedRouteActivity : MvxActivity<RecommendedRouteViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.RecommendedRouteActivity);

            var set = CreateBindingSet();

            set.Bind(FindViewById(Resource.Id.next_button))
                .For(x => x.BindClick())
                .To(vm => vm.NextStepCommand);

            set.Bind(FindViewById(Resource.Id.nav_back_button))
                .For(x => x.BindClick())
                .To(vm => vm.NavigateBackCommand);

            set.Apply();
        }
    }
}

