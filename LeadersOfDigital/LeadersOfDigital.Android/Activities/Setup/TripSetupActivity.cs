using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using LeadersOfDigital.Converters;
using LeadersOfDigital.ViewModels.Setup;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace LeadersOfDigital.Android.Activities.Setup
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class TripSetupActivity : MvxActivity<TripSetupViewModel>
    {
        private LinearLayout _tripEndLayout;
        private LinearLayout _tripStartLayout;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.TripSetupActivity);

            var set = CreateBindingSet();

            set.Bind(FindViewById<Button>(Resource.Id.find_tickets_button))
                .For(x => x.BindClick())
                .To(vm => vm.FindTicketsCommand);

            set.Bind(FindViewById<Button>(Resource.Id.add_friend_button))
                .For(x => x.BindClick())
                .To(vm => vm.AddFriendCommand);

            set.Bind(FindViewById<Button>(Resource.Id.nav_back_button))
                .For(x => x.BindClick())
                .To(vm => vm.NavigateBackCommand);

            (_tripEndLayout = FindViewById<LinearLayout>(Resource.Id.trip_end_layout)).Click += TripEndLayoutClick;

            (_tripStartLayout = FindViewById<LinearLayout>(Resource.Id.trip_start_layout)).Click += TripStartLayoutClick;

            set.Bind(FindViewById<TextView>(Resource.Id.trip_start_date_textview))
                .For(x => x.Text)
                .To(vm => vm.StartDate)
                .WithConversion<NullDateToStringConverter>()
                .OneWay();

            set.Bind(FindViewById<TextView>(Resource.Id.trip_end_date_textview))
                .For(x => x.Text)
                .To(vm => vm.EndDate)
                .WithConversion<NullDateToStringConverter>()
                .OneWay();

            set.Apply();
        }

        protected override void OnDestroy()
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
            => new DatePickerDialog(this, (_, e) => dateSelected(e.Date), startDate.Year, startDate.Month - 1, startDate.Day).Show();
    }
}

