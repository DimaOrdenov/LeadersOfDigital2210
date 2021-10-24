using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using Converters;
using Definitions.Interactions;
using LeadersOfDigital.Android.Helpers;
using LeadersOfDigital.ViewModels.Setup;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace LeadersOfDigital.Android.Activities.Setup
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class ChooseHotelsActivity : MvxActivity<ChooseHotelsViewModel>
    {
        private Button _back;
        private TextView _firstFlightDate;
        private TextView _lastFlightDate;
        private TextView _firstFlightRoute;
        private TextView _lastFlightRoute;
        private TextView _firstFlightTimeRange;
        private TextView _lastFlightTimeRange;
        private TextView _suggestionPrice;
        private TextView _suggestionName;

        private IExtendedInteraction<BaseInteractionResult> _commonExceptionInteraction;
        private IExtendedInteraction<BaseInteractionResult> _humanReadableExceptionInteraction;
        private ConstraintLayout _nextStep;

        public IExtendedInteraction<BaseInteractionResult> CommonExceptionInteraction
        {
            get => _commonExceptionInteraction;
            set => this.SetInteraction(ref _commonExceptionInteraction, value);
        }

        public IExtendedInteraction<BaseInteractionResult> HumanReadableExceptionInteraction
        {
            get => _humanReadableExceptionInteraction;
            set => this.SetInteraction(ref _humanReadableExceptionInteraction, value);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ChooseHotelsView);

            _back = FindViewById<Button>(Resource.Id.choose_hotels_back);
            _firstFlightDate = FindViewById<TextView>(Resource.Id.choose_hotels_panel_to_date);
            _lastFlightDate = FindViewById<TextView>(Resource.Id.choose_hotels_panel_from_date);
            _firstFlightRoute = FindViewById<TextView>(Resource.Id.choose_hotels_panel_to_route);
            _lastFlightRoute = FindViewById<TextView>(Resource.Id.choose_hotels_panel_from_route);
            _firstFlightTimeRange = FindViewById<TextView>(Resource.Id.choose_hotels_panel_to_time_range);
            _lastFlightTimeRange = FindViewById<TextView>(Resource.Id.choose_hotels_panel_from_time_range);
            _suggestionPrice = FindViewById<TextView>(Resource.Id.choose_hotels_suggestion_price);
            _suggestionName = FindViewById<TextView>(Resource.Id.choose_hotels_suggestion_name);
            _nextStep = FindViewById<ConstraintLayout>(Resource.Id.choose_hotels_next_step);

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

            set.Bind(_suggestionPrice)
                .For(x => x.Text)
                .To(vm => vm.SuggestedHotel.PriceAvg)
                .WithConversion<StringFormatConverter>("{0}₽");

            set.Bind(_suggestionName)
                .For(x => x.Text)
                .To(vm => vm.SuggestedHotel.HotelName);

            set.Bind(_nextStep)
                .For(x => x.BindClick())
                .To(vm => vm.NextStepCommand);

            set.Bind(this)
                .For(x => x.CommonExceptionInteraction)
                .To(vm => vm.CommonExceptionInteraction)
                .OneWay();

            set.Bind(this)
                .For(x => x.HumanReadableExceptionInteraction)
                .To(vm => vm.HumanReadableExceptionInteraction)
                .OneWay();

            set.Apply();

            this.AddLoadingStateActivityBindings<ChooseHotelsActivity, ChooseHotelsViewModel>();
        }
    }
}
