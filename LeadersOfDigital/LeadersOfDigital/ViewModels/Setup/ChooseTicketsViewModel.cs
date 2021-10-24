using System;
using System.Linq;
using System.Threading.Tasks;
using Business;
using Definitions.Interactions;
using LeadersOfDigital.Definitions;
using LeadersOfDigital.Definitions.Exceptions;
using LeadersOfDigital.Helpers;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Xamarin.Essentials;

namespace LeadersOfDigital.ViewModels.Setup
{
    public class ChooseTicketsViewModel : PageViewModel<ChooseTicketsViewModel.ChooseTicketsParameter, ViewModelResult>
    {
        private readonly IFlightsService _flightsService;
        private TicketItemViewModel _departureTicket;
        private TicketItemViewModel _arrivalTicket;

        public ChooseTicketsViewModel(
            IFlightsService flightsService,
            IMvxNavigationService navigationService,
            ILogger<ChooseTicketsViewModel> logger)
            : base(navigationService, logger)
        {
            _flightsService = flightsService;

            NextStepCommand = new MvxAsyncCommand(item => navigationService.Navigate<InterestsSetupViewModel>());

            ChooseDepartureTicketCommand = new MvxCommand<TicketItemViewModel>(
                async item =>
                {
                    DepartureTicket = item;

                    TicketsResults.Clear();
                    await RaisePropertyChanged(nameof(TicketsResults));

                    await SearchTicketsAsync(1, 0, NavigationParameter.ArrivesAt);
                });

            ChooseArrivalTicketCommand = new MvxCommand<TicketItemViewModel>(
                async item =>
                {
                    ArrivalTicket = item;

                    TicketsResults.Clear();

                    TicketsResults.Add(DepartureTicket);
                    TicketsResults.Add(ArrivalTicket);

                    await RaisePropertyChanged(nameof(TicketsResults));
                });

            NextStepCommand = new MvxCommand(
                async () =>
                {
                    if (_departureTicket == null || _arrivalTicket == null)
                    {
                        HumanReadableExceptionInteractionLocal.Raise(new BaseInteractionResult(false) { ErrorMessage = "Сначала выберите перелет туда и обратно" });

                        return;
                    }

                    var result = await NavigationService.Navigate<ChooseHotelsViewModel, ChooseHotelsViewModel.ChooseHotelsParameter, ViewModelResult>(
                        new ChooseHotelsViewModel.ChooseHotelsParameter(
                            NavigationParameter.DepartsAt,
                            NavigationParameter.ArrivesAt,
                            DepartureTicket,
                            ArrivalTicket));

                    if (result?.CloseRequested == true)
                    {
                        await Task.Delay(200);
                        await NavigationService.Close(this, new ViewModelResult(true));
                    }
                });

            TicketsResults = new MvxObservableCollection<TicketItemViewModel>();
        }

        public IMvxCommand<TicketItemViewModel> ChooseDepartureTicketCommand { get; }

        public IMvxCommand<TicketItemViewModel> ChooseArrivalTicketCommand { get; }

        public IMvxCommand NextStepCommand { get; }

        public MvxObservableCollection<TicketItemViewModel> TicketsResults { get; }

        public TicketItemViewModel DepartureTicket
        {
            get => _departureTicket;
            set => SetProperty(ref _departureTicket, value);
        }

        public TicketItemViewModel ArrivalTicket
        {
            get => _arrivalTicket;
            set => SetProperty(ref _arrivalTicket, value);
        }

        public override IMvxCommand NavigateBackCommand { get; }

        public override void OnHardwareBackPressed()
        {
            NavigationService.Close(this, new ViewModelResult());
        }

        public override void ViewCreated()
        {
            base.ViewCreated();

            Task.Run(async () => await SearchTicketsAsync(0, 1, NavigationParameter.DepartsAt));
        }

        private async Task SearchTicketsAsync(int originId, int destinationId, DateTime date)
        {
            State = PageStateType.Loading;

            await PerformAsync(new ViewModelHandledAction(
                    async () =>
                    {
                        var flights = (await _flightsService.GetFlightsAsync(
                                originId,
                                destinationId,
                                date,
                                CancellationToken))?
                            .ToList();

                        if (flights is not { Count: > 0 })
                        {
                            throw new HumanReadableException("На выбранные даты нет рейсов");
                        }

                        TicketsResults.AddRange(flights.Select(x =>
                        {
                            var viewModel = new TicketItemViewModel(x);

                            viewModel.BuyCommand = new MvxCommand<TicketItemViewModel>(
                                async _ =>
                                {
                                    if (DepartureTicket != null && ArrivalTicket != null)
                                    {
                                        return;
                                    }

                                    await Browser.OpenAsync(x.Link, new BrowserLaunchOptions
                                    {
                                        LaunchMode = BrowserLaunchMode.SystemPreferred,
                                    });

                                    if (DepartureTicket == null)
                                    {
                                        ChooseDepartureTicketCommand.Execute(viewModel);
                                    }
                                    else if (ArrivalTicket == null)
                                    {
                                        ChooseArrivalTicketCommand.Execute(viewModel);
                                    }
                                });

                            return viewModel;
                        }).ToMvxObservableCollection());
                        await RaisePropertyChanged(nameof(TicketsResults));
                    })
                .SetCommonErrorMessage("Не удалось найти рейсы")
                .SetIsWithInteraction(true));

            State = PageStateType.Content;
        }

        public class ChooseTicketsParameter
        {
            public ChooseTicketsParameter(DateTime departsAt, DateTime arrivesAt)
            {
                DepartsAt = departsAt;
                ArrivesAt = arrivesAt;
            }

            public DateTime DepartsAt { get; }

            public DateTime ArrivesAt { get; }
        }
    }
}
