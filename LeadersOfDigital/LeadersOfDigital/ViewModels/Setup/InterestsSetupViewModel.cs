using System;
using LeadersOfDigital.DataModels.Responses.Hotels;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels.Setup
{
    public class InterestsSetupViewModel : PageViewModel<InterestsSetupViewModel.InterestsSetupParameter>
    {
        public IMvxAsyncCommand NextStepCommand { get; }

        public InterestsSetupViewModel(IMvxNavigationService navigationService, ILogger<InterestsSetupViewModel> logger)
            : base(navigationService, logger)
        {
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
