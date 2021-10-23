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

        public MainViewPagerAdapter(FragmentActivity activity, int itemsCount)
            : base(activity)
        {
            ItemCount = itemsCount;
            _fragmentsArray = new Fragment[ItemCount];
        }

        public override int ItemCount { get; }

        public override Fragment CreateFragment(int p0) =>
            p0 switch
            {
                0 => _fragmentsArray[p0] ?? (_fragmentsArray[p0] = new MainFragment { ViewModel = Mvx.IoCProvider.Resolve<MainViewModel>() }),
                1 => _fragmentsArray[p0] ?? (_fragmentsArray[p0] = new FavoritesFragment { ViewModel = Mvx.IoCProvider.Resolve<FavoritesViewModel>() }),
                2 => _fragmentsArray[p0] ?? (_fragmentsArray[p0] = new PopularFragment { ViewModel = Mvx.IoCProvider.Resolve<PopularViewModel>() }),
                3 => _fragmentsArray[p0] ?? (_fragmentsArray[p0] = new ProfileFragment { ViewModel = Mvx.IoCProvider.Resolve<ProfileViewModel>() }),
                _ => throw new NotImplementedException(),
            };
    }
}