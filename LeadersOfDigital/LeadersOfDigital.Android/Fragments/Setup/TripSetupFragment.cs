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
using static Android.App.DatePickerDialog;

namespace LeadersOfDigital.Android.Fragments.Setup
{
    [MvxFragmentPresentation]
    public class TripSetupFragment : MvxFragment<TripSetupViewModel>
    {
        private LinearLayout _tripEndLayout;
        private LinearLayout _tripStartLayout;
        private TextView _tripStartDateTextView;
        private TextView _tripEndDateTextView;

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

            set.Bind(_tripStartDateTextView = view.FindViewById<TextView>(Resource.Id.trip_start_date_textview))
                .For(x => x.Text)
                .To(vm => vm.StartDate)
                .WithConversion<StringToNullDateConverter>()
                .OneWayToSource();

            set.Bind(_tripEndDateTextView = view.FindViewById<TextView>(Resource.Id.trip_end_date_textview))
                .For(x => x.Text)
                .To(vm => vm.EndDate)
                .WithConversion<StringToNullDateConverter>()
                .OneWayToSource();

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
            => ShowDatePickerDialog(_tripEndDateTextView);

        private void TripStartLayoutClick(object sender, System.EventArgs e)
            => ShowDatePickerDialog(_tripStartDateTextView);

        private void ShowDatePickerDialog(TextView textView)
        {
            var date = TryParseDateOrToday(textView.Text);
            var dialog = new DatePickerDialog(Context, default(IOnDateSetListener), date.Year, date.Month, date.Day);
            dialog.DateSet += DialogDateSet;
            dialog.Show();

            void DialogDateSet(object sender, DateSetEventArgs e)
            {
                textView.Text = e.Date.ToString("d");

                dialog.DateSet -= DialogDateSet;
            }
        }

        private DateTime TryParseDateOrToday(string dateStr) => DateTime.TryParse(dateStr, out var date) ? date : DateTime.Today;
    }
}

