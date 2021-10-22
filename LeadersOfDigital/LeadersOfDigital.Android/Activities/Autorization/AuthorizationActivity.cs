using Android.App;
using Android.OS;
using Android.Widget;
using LeadersOfDigital.ViewModels.Authorization;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace LeadersOfDigital.Android.Activities
{
    [MvxActivityPresentation]
    [Activity(Label = "AuthorizationActivity")]
    public class AuthorizationActivity : MvxActivity<AuthorizationViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.AuthorizationActivity);

            var set = CreateBindingSet();

            set.Bind(FindViewById<Button>(Resource.Id.authorize_button))
                .For(x => x.BindClick())
                .To(vm => vm.AuthorizationCommand);

            set.Bind(FindViewById<TextView>(Resource.Id.skip_auth_textview))
                .For(x => x.BindClick())
                .To(vm => vm.SkipAuthCommand);

           foreach (var networkButton in new[]
           {
                FindViewById<ImageButton>(Resource.Id.odnoklassniki_button),
                FindViewById<ImageButton>(Resource.Id.sbarbank_button),
                FindViewById<ImageButton>(Resource.Id.vk_button),
                FindViewById<ImageButton>(Resource.Id.telegram_button),
                FindViewById<ImageButton>(Resource.Id.yandex_button)
            })
            {
                set.Bind(networkButton)
                   .For(x => x.BindClick())
                   .To(vm => vm.AuthViaSocialNetworkCommand);
            }

            set.Apply();
        }
    }
}
