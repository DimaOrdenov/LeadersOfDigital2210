using System;
using System.Threading.Tasks;
using LeadersOfDigital.DataModels.Responses.Hotels;
using LeadersOfDigital.Definitions;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels.Setup
{
    public class InterestsSetupViewModel : PageViewModel<InterestsSetupViewModel.InterestsSetupParameter>
    {
        public InterestsSetupViewModel(
            AppStorage appStorage,
            IMvxNavigationService navigationService,
            ILogger<InterestsSetupViewModel> logger)
            : base(navigationService, logger)
        {
            NextStepCommand = new MvxCommand(
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

                    await NavigationService.Close(this);
                    await Task.Delay(500);
                    await NavigationService.Close(this);
                    await Task.Delay(500);
                    await NavigationService.Close(this);
                    await Task.Delay(500);
                    await NavigationService.Close(this);
                });
        }

        public IMvxCommand NextStepCommand { get; }

        public class InterestsSetupParameter : ChooseHotelsViewModel.ChooseHotelsParameter
        {
            public InterestsSetupParameter(DateTime departsAt, DateTime arrivesAt, TicketItemViewModel flightFrom, TicketItemViewModel flightTo, HotelsResponse hotelsResponse)
                : base(departsAt, arrivesAt, flightFrom, flightTo)
            {
                HotelsResponse = hotelsResponse;
            }

            public HotelsResponse HotelsResponse { get; }
        }
    }
}
