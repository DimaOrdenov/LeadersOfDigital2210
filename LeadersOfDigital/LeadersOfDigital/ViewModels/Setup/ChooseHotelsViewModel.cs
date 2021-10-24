using System;
using System.Threading.Tasks;
using Business;
using LeadersOfDigital.DataModels.Responses.Hotels;
using LeadersOfDigital.Definitions;
using LeadersOfDigital.Definitions.Exceptions;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels.Setup
{
    public class ChooseHotelsViewModel : PageViewModel<ChooseHotelsViewModel.ChooseHotelsParameter, ViewModelResult>
    {
        private readonly IHotelsService _hotelsService;
        private HotelsResponse _suggestedHotel;

        public ChooseHotelsViewModel(
            IHotelsService hotelsService,
            IMvxNavigationService navigationService,
            ILogger<ChooseHotelsViewModel> logger)
            : base(navigationService, logger)
        {
            _hotelsService = hotelsService;

            NextStepCommand = new MvxCommand(
                async () =>
                {
                    var result = await NavigationService.Navigate<InterestsSetupViewModel, InterestsSetupViewModel.InterestsSetupParameter, ViewModelResult>(
                        new InterestsSetupViewModel.InterestsSetupParameter(
                            NavigationParameter.DepartsAt,
                            NavigationParameter.ArrivesAt,
                            NavigationParameter.FlightFrom,
                            NavigationParameter.FlightTo,
                            _suggestedHotel));

                    if (result?.CloseRequested == true)
                    {
                        await Task.Delay(200);
                        await NavigationService.Close(this, new ViewModelResult(true));
                    }
                });
            
            NavigateBackCommand = new MvxCommand(() => navigationService.Close(this, new ViewModelResult()));
        }

        public IMvxCommand NextStepCommand { get; }

        public HotelsResponse SuggestedHotel
        {
            get => _suggestedHotel;
            set => SetProperty(ref _suggestedHotel, value);
        }

        public override void ViewCreated()
        {
            base.ViewCreated();

            Task.Run(async () =>
            {
                State = PageStateType.Loading;

                await PerformAsync(
                    new ViewModelHandledAction(
                            async () =>
                            {
                                var hotel = await _hotelsService.GetSuggestedHotelAsync(100000, 15000, 1, NavigationParameter.DepartsAt, NavigationParameter.ArrivesAt, CancellationToken);

                                SuggestedHotel = hotel ?? throw new HumanReadableException("Не получилось подобрать отель");
                            })
                        .SetCommonErrorMessage("Ошибка подбора отеля")
                        .SetIsWithInteraction(true));

                State = PageStateType.Content;
            });
        }

        public override IMvxCommand NavigateBackCommand { get; }

        public override void OnHardwareBackPressed()
        {
            NavigationService.Close(this, new ViewModelResult());
        }

        public class ChooseHotelsParameter : ChooseTicketsViewModel.ChooseTicketsParameter
        {
            public ChooseHotelsParameter(DateTime departsAt, DateTime arrivesAt, TicketItemViewModel flightFrom, TicketItemViewModel flightTo)
                : base(departsAt, arrivesAt)
            {
                FlightFrom = flightFrom;
                FlightTo = flightTo;
            }

            public TicketItemViewModel FlightTo { get; }

            public TicketItemViewModel FlightFrom { get; }
        }
    }
}
