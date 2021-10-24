using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using Converters;
using LeadersOfDigital.ViewModels.Setup;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace LeadersOfDigital.Android.Activities.Setup
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class InterestsSetupActivity : MvxActivity<InterestsSetupViewModel>
    {
        private Button _back;
        private TextView _firstFlightDate;
        private TextView _lastFlightDate;
        private TextView _firstFlightRoute;
        private TextView _lastFlightRoute;
        private TextView _firstFlightTimeRange;
        private TextView _lastFlightTimeRange;
        private TextView _hotelName;
        private TextView _hotelLastDate;
        private ConstraintLayout _nextStep;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.InterestsSetupActivity);

            _back = FindViewById<Button>(Resource.Id.nav_back_button);
            _firstFlightDate = FindViewById<TextView>(Resource.Id.choose_interests_panel_to_date);
            _lastFlightDate = FindViewById<TextView>(Resource.Id.choose_interests_panel_from_date);
            _firstFlightRoute = FindViewById<TextView>(Resource.Id.choose_interests_panel_to_route);
            _lastFlightRoute = FindViewById<TextView>(Resource.Id.choose_interests_panel_from_route);
            _firstFlightTimeRange = FindViewById<TextView>(Resource.Id.choose_interests_panel_to_time_range);
            _lastFlightTimeRange = FindViewById<TextView>(Resource.Id.choose_interests_panel_from_time_range);
            _hotelName = FindViewById<TextView>(Resource.Id.choose_interests_panel_hotel_name);
            _hotelLastDate = FindViewById<TextView>(Resource.Id.choose_interests_panel_hotel_date);
            _nextStep = FindViewById<ConstraintLayout>(Resource.Id.choose_interests_next_step);

            var set = CreateBindingSet();

            set.Bind(_back)
                .For(x => x.BindClick())
                .To(vm => vm.NavigateBackCommand);

            set.Bind(_firstFlightDate)
                .For(x => x.Text)
                .To(vm => vm.NavigationParameter.FlightFrom.FlightsResponse.Departure_at)
                .WithConversion<StringFormatConverter>("{0:dd.MM}");

            set.Bind(_lastFlightDate)
                .For(x => x.Text)
                .To(vm => vm.NavigationParameter.FlightTo.FlightsResponse.Departure_at)
                .WithConversion<StringFormatConverter>("{0:dd.MM}");

            set.Bind(_firstFlightRoute)
                .For(x => x.Text)
                .To(vm => vm.NavigationParameter.FlightFrom.Route);

            set.Bind(_lastFlightRoute)
                .For(x => x.Text)
                .To(vm => vm.NavigationParameter.FlightTo.Route);

            set.Bind(_firstFlightTimeRange)
                .For(x => x.Text)
                .To(vm => vm.NavigationParameter.FlightFrom.TimeRange);

            set.Bind(_lastFlightTimeRange)
                .For(x => x.Text)
                .To(vm => vm.NavigationParameter.FlightTo.TimeRange);

            set.Bind(_hotelLastDate)
                .For(x => x.Text)
                .To(vm => vm.NavigationParameter.FlightTo.FlightsResponse.Departure_at)
                .WithConversion<StringFormatConverter>("{0:dd.MM}");

            set.Bind(_hotelName)
                .For(x => x.Text)
                .To(vm => vm.NavigationParameter.HotelsResponse.HotelName);

            set.Bind(_nextStep)
                .For(x => x.BindClick())
                .To(vm => vm.NextStepCommand);

            set.Apply();
        }
    }
}
