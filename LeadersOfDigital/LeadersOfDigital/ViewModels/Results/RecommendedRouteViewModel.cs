using System;
using System.Threading.Tasks;
using LeadersOfDigital.DataModels.Responses.Hotels;
using LeadersOfDigital.Definitions;
using LeadersOfDigital.ViewModels.Setup;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels.Results
{
    public class RecommendedRouteViewModel : PageViewModel<RecommendedRouteViewModel.RecommendedRouteParameter, ViewModelResult>
    {
        public IMvxAsyncCommand NextStepCommand { get; }

        public RecommendedRouteViewModel(
            AppStorage appStorage,
            IMvxNavigationService navigationService,
            ILogger<RecommendedRouteViewModel> logger)
            : base(navigationService, logger)
        {
            NextStepCommand = new MvxAsyncCommand(
                async () =>
                {
                    appStorage.PlannedTrip = new Trip
                    {
                        StartsAt = NavigationParameter.DepartsAt,
                        EndsAt = NavigationParameter.ArrivesAt,
                        FlightFrom = NavigationParameter.FlightFrom,
                        FlightTo = NavigationParameter.FlightTo,
                        Hotel = NavigationParameter.HotelsResponse,
                    };

                    await NavigationService.Close(this, new ViewModelResult(true));
                });

            NavigateBackCommand = new MvxCommand(() => navigationService.Close(this, new ViewModelResult()));
        }

        public override IMvxCommand NavigateBackCommand { get; }

        public override void OnHardwareBackPressed()
        {
            NavigationService.Close(this, new ViewModelResult());
        }

        public class RecommendedRouteParameter : InterestsSetupViewModel.InterestsSetupParameter
        {
            public RecommendedRouteParameter(DateTime departsAt, DateTime arrivesAt, TicketItemViewModel flightFrom, TicketItemViewModel flightTo, HotelsResponse hotelsResponse)
                : base(departsAt, arrivesAt, flightFrom, flightTo, hotelsResponse)
            {
            }
        }
    }
}
