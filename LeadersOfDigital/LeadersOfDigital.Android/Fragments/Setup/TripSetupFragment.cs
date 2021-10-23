using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using LeadersOfDigital.Converters;
using LeadersOfDigital.ViewModels.Setup;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace LeadersOfDigital.Android.Fragments.Setup
{
    [MvxFragmentPresentation]
    public class TripSetupFragment : MvxFragment<TripSetupViewModel>
    {
        private LinearLayout _tripEndLayout;
        private LinearLayout _tripStartLayout;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.TripSetupFragment, container, false);
            var set = CreateBindingSet();

            set.Bind(view.FindViewById<Button>(Resource.Id.find_tickets_button))
                .For(x => x.BindClick())
                .To(vm => vm.FindTicketsCommand);

            set.Bind(view.FindViewById<Button>(Resource.Id.add_friend_button))
                .For(x => x.BindClick())
                .To(vm => vm.AddFriendCommand);

            (_tripEndLayout = view.FindViewById<LinearLayout>(Resource.Id.trip_end_layout)).Click += TripEndLayoutClick;

            (_tripStartLayout = view.FindViewById<LinearLayout>(Resource.Id.trip_start_layout)).Click += TripStartLayoutClick;

            set.Bind(view.FindViewById<TextView>(Resource.Id.trip_start_date_textview))
                .For(x => x.Text)
                .To(vm => vm.StartDate)
                .WithConversion<NullDateToStringConverter>()
                .OneWay();

            set.Bind(view.FindViewById<TextView>(Resource.Id.trip_end_date_textview))
                .For(x => x.Text)
                .To(vm => vm.EndDate)
                .WithConversion<NullDateToStringConverter>()
                .OneWay();

            set.Apply();

            return view;
        }

        public override void OnDestroy()
        {
            _tripEndLayout.Click -= TripEndLayoutClick;
            _tripStartLayout.Click -= TripStartLayoutClick;

            base.OnDestroy();
        }

        private void TripEndLayoutClick(object sender, EventArgs e)
            => ShowDatePickerDialog(ViewModel.EndDate ?? DateTime.Today, selectedDate => ViewModel.EndDate = selectedDate);

        private void TripStartLayoutClick(object sender, EventArgs e)
            => ShowDatePickerDialog(ViewModel.StartDate ?? DateTime.Today, selectedDate => ViewModel.StartDate = selectedDate);

        private void ShowDatePickerDialog(DateTime startDate, Action<DateTime> dateSelected)
            => new DatePickerDialog(Context, (_, e) => dateSelected(e.Date), startDate.Year, startDate.Month - 1, startDate.Day).Show();
    }
}

