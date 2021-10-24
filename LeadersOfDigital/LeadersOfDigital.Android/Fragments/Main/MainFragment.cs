using System.ComponentModel;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using Converters;
using LeadersOfDigital.Converters;
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
        private LinearLayout _noTripsContainer;
        private ConstraintLayout _whatToTake;
        private ConstraintLayout _pharmacy;
        private ConstraintLayout _plannedTrip;
        private TextView _firstFlightDate;
        private TextView _lastFlightDate;
        private TextView _firstFlightRoute;
        private TextView _lastFlightRoute;
        private TextView _firstFlightTimeRange;
        private TextView _lastFlightTimeRange;
        private TextView _hotelName;
        private TextView _hotelPrice;
        private Button _morePlan;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.MainFragment, container, false);

            _noTripsContainer = view.FindViewById<LinearLayout>(Resource.Id.no_trips_view);
            _whatToTake = view.FindViewById<ConstraintLayout>(Resource.Id.main_what_to_take);
            _pharmacy = view.FindViewById<ConstraintLayout>(Resource.Id.main_pharmacy);
            _plannedTrip = view.FindViewById<ConstraintLayout>(Resource.Id.planned_trip_panel);
            _firstFlightDate = view.FindViewById<TextView>(Resource.Id.planned_trip_panel_to_date);
            _lastFlightDate = view.FindViewById<TextView>(Resource.Id.planned_trip_panel_from_date);
            _firstFlightRoute = view.FindViewById<TextView>(Resource.Id.planned_trip_panel_to_route);
            _lastFlightRoute = view.FindViewById<TextView>(Resource.Id.planned_trip_panel_from_route);
            _firstFlightTimeRange = view.FindViewById<TextView>(Resource.Id.planned_trip_panel_to_time_range);
            _lastFlightTimeRange = view.FindViewById<TextView>(Resource.Id.planned_trip_panel_from_time_range);
            _hotelName = view.FindViewById<TextView>(Resource.Id.planned_trip_panel_hotel_name);
            _hotelPrice = view.FindViewById<TextView>(Resource.Id.planned_trip_panel_hotel_price);
            _morePlan = view.FindViewById<Button>(Resource.Id.main_more_plan);

            var set = CreateBindingSet();

            set.Bind(view.FindViewById<Button>(Resource.Id.start_planning_button))
                .For(x => x.BindClick())
                .To(vm => vm.StartPlanningCommand);

            set.Bind(_noTripsContainer)
                .For(x => x.BindVisible())
                .To(vm => vm.AppStorage.PlannedTrip)
                .WithConversion<IsNullConverter>();

            set.Bind(_whatToTake)
                .For(x => x.BindVisible())
                .To(vm => vm.AppStorage.PlannedTrip)
                .WithConversion<IsNotNullConverter>();

            set.Bind(_pharmacy)
                .For(x => x.BindVisible())
                .To(vm => vm.AppStorage.PlannedTrip)
                .WithConversion<IsNotNullConverter>();

            set.Bind(_plannedTrip)
                .For(x => x.BindVisible())
                .To(vm => vm.AppStorage.PlannedTrip)
                .WithConversion<IsNotNullConverter>();

            set.Bind(_morePlan)
                .For(x => x.BindVisible())
                .To(vm => vm.AppStorage.PlannedTrip)
                .WithConversion<IsNotNullConverter>();

            set.Bind(_morePlan)
                .For(x => x.BindClick())
                .To(vm => vm.StartPlanningCommand);

            set.Bind(_firstFlightDate)
                .For(x => x.Text)
                .To(vm => vm.AppStorage.PlannedTrip.FlightFrom.FlightsResponse.Departure_at)
                .WithConversion<StringFormatConverter>("{0:dd.MM}");

            set.Bind(_lastFlightDate)
                .For(x => x.Text)
                .To(vm => vm.AppStorage.PlannedTrip.FlightTo.FlightsResponse.Departure_at)
                .WithConversion<StringFormatConverter>("{0:dd.MM}");

            set.Bind(_firstFlightRoute)
                .For(x => x.Text)
                .To(vm => vm.AppStorage.PlannedTrip.FlightFrom.Route);

            set.Bind(_lastFlightRoute)
                .For(x => x.Text)
                .To(vm => vm.AppStorage.PlannedTrip.FlightTo.Route);

            set.Bind(_firstFlightTimeRange)
                .For(x => x.Text)
                .To(vm => vm.AppStorage.PlannedTrip.FlightFrom.TimeRange);

            set.Bind(_lastFlightTimeRange)
                .For(x => x.Text)
                .To(vm => vm.AppStorage.PlannedTrip.FlightTo.TimeRange);

            set.Bind(_hotelName)
                .For(x => x.Text)
                .To(vm => vm.AppStorage.PlannedTrip.Hotel.HotelName);

            set.Bind(_hotelPrice)
                .For(x => x.Text)
                .To(vm => vm.AppStorage.PlannedTrip.Hotel.PriceAvg)
                .WithConversion<StringFormatConverter>("{0} ₽");

            set.Apply();

            ViewModel.AppStorage.PropertyChanged += AppStorageOnPropertyChanged;

            return view;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            if (ViewModel?.AppStorage != null)
            {
                ViewModel.AppStorage.PropertyChanged -= AppStorageOnPropertyChanged;
            }
        }

        private void AppStorageOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.AppStorage.PlannedTrip):

                    break;
            }
        }
    }
}
