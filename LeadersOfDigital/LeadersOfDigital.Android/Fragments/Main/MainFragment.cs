using Android.OS;
using Android.Views;
using Android.Widget;
using LeadersOfDigital.ViewModels.Main;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace LeadersOfDigital.Android.Fragments.Main
{
    [MvxFragmentPresentation]
    public class MainFragment : MvxFragment<MainViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.MainFragment, container, false);

            var set = CreateBindingSet();

            set.Bind(view.FindViewById<Button>(Resource.Id.start_planning_button))
                .For(x => x.BindClick())
                .To(vm => vm.StartPlanningCommand);

            set.Apply();

            return view;
        }
    }
}
