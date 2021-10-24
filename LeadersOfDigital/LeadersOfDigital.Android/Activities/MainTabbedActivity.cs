using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.BottomNavigation;
using LeadersOfDigital.Android.Adapters.Main;
using LeadersOfDigital.ViewModels;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace LeadersOfDigital.Android.Activities
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainTabbedActivity : MvxActivity<MainTabbedViewModel>
    {
        private ViewPager2 _viewPager;
        private BottomNavigationView _navigationView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainTabbedActivity);

            _viewPager = FindViewById<ViewPager2>(Resource.Id.main_view_pager);
            _navigationView = FindViewById<BottomNavigationView>(Resource.Id.bottom_nav);

            _viewPager.UserInputEnabled = false;
            _viewPager.Adapter = new MainViewPagerAdapter(this, 4);

            _navigationView.ItemSelected += NavigationView_ItemSelected;
        }

        private void NavigationView_ItemSelected(object sender, Google.Android.Material.Navigation.NavigationBarView.ItemSelectedEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.action_main:
                    _viewPager?.SetCurrentItem(0, true);

                    break;
                case Resource.Id.action_favorites:
                    _viewPager?.SetCurrentItem(1, true);

                    break;
                case Resource.Id.action_popular:
                    _viewPager?.SetCurrentItem(2, true);

                    break;
                case Resource.Id.action_profile:
                    _viewPager?.SetCurrentItem(3, true);

                    break;
            }
        }

        protected override void OnDestroy()
        {
            _navigationView.ItemSelected -= NavigationView_ItemSelected;
            base.OnDestroy();
        }
    }
}
