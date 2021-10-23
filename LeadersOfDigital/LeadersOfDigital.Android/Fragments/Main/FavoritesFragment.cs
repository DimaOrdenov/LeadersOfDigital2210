using Android.OS;
using Android.Views;
using LeadersOfDigital.ViewModels.Main;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace LeadersOfDigital.Android.Fragments.Main
{
    [MvxFragmentPresentation]
    public class FavoritesFragment : MvxFragment<FavoritesViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.FavoritesFragment, container, false);
        } 
    }
}

