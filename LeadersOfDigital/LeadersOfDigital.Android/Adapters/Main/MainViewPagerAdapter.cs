using System;
using AndroidX.Fragment.App;
using AndroidX.ViewPager2.Adapter;
using LeadersOfDigital.Android.Fragments.Main;
using LeadersOfDigital.ViewModels.Main;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Navigation;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace LeadersOfDigital.Android.Adapters.Main
{
    public class MainViewPagerAdapter : FragmentStateAdapter
    {
        private Fragment[] _fragmentsArray;
        private readonly FragmentActivity _activity;

        public MainViewPagerAdapter(FragmentActivity activity, int itemsCount)
            : base(activity)
        {
            ItemCount = itemsCount;
            _activity = activity;
            _fragmentsArray = new Fragment[ItemCount];
        }

        public override int ItemCount { get; }

        public override Fragment CreateFragment(int p0) =>
            p0 switch
            {
                0 => _fragmentsArray[p0] ?? (_fragmentsArray[p0] = new MainFragment { ViewModel = new MainViewModel(Mvx.IoCProvider.Resolve<IMvxNavigationService>(), Mvx.IoCProvider.Resolve<ILogger<MainViewModel>>()) }),
                1 => _fragmentsArray[p0] ?? (_fragmentsArray[p0] = new FavoritesFragment { ViewModel =  new FavoritesViewModel(Mvx.IoCProvider.Resolve<IMvxNavigationService>(), Mvx.IoCProvider.Resolve<ILogger<FavoritesViewModel>>()) }),
                2 => _fragmentsArray[p0] ?? (_fragmentsArray[p0] = new PopularFragment { ViewModel = new PopularViewModel(Mvx.IoCProvider.Resolve<IMvxNavigationService>(), Mvx.IoCProvider.Resolve<ILogger<PopularViewModel>>()) }),
                3 => _fragmentsArray[p0] ?? (_fragmentsArray[p0] = new ProfileFragment { ViewModel = new ProfileViewModel(Mvx.IoCProvider.Resolve<IMvxNavigationService>(), Mvx.IoCProvider.Resolve<ILogger<ProfileViewModel>>()) }),
                _ => throw new NotImplementedException(),
            };
    }
}