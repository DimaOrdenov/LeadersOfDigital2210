using System;
using System.Threading.Tasks;
using LeadersOfDigital.ViewModels.Results;
using LeadersOfDigital.DataModels.Responses.Hotels;
using LeadersOfDigital.Definitions;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels.Setup
{
    public class InterestsSetupViewModel : PageViewModel<InterestsSetupViewModel.InterestsSetupParameter, ViewModelResult>
    {
        public InterestsSetupViewModel(
            IMvxNavigationService navigationService,
            ILogger<InterestsSetupViewModel> logger)
            : base(navigationService, logger)
        {
            NextStepCommand = new MvxCommand(
                async () =>
                {
                    var result = await NavigationService.Navigate<RecommendedRouteViewModel, RecommendedRouteViewModel.RecommendedRouteParameter, ViewModelResult>(
                        new RecommendedRouteViewModel.RecommendedRouteParameter(
                            NavigationParameter.DepartsAt,
                            NavigationParameter.ArrivesAt,
                            NavigationParameter.FlightFrom,
                            NavigationParameter.FlightTo,
                            NavigationParameter.HotelsResponse
                        ));

                    if (result?.CloseRequested == true)
                    {
                        await Task.Delay(200);
                        await NavigationService.Close(this, new ViewModelResult(true));
                    }
                });

            NavigateBackCommand = new MvxCommand(() => navigationService.Close(this, new ViewModelResult()));
        }

        public IMvxCommand NextStepCommand { get; }

        public override IMvxCommand NavigateBackCommand { get; }

        public override void OnHardwareBackPressed()
        {
            NavigationService.Close(this, new ViewModelResult());
        }

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
