using System;
using System.Threading.Tasks;
using LeadersOfDigital.Definitions;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels.Setup
{
    public class TripSetupViewModel : PageViewModel<string, ViewModelResult>
    {
        private DateTime? _startDate;
        private DateTime? _endDate;

        public IMvxAsyncCommand AddFriendCommand { get; }

        public IMvxAsyncCommand FindTicketsCommand { get; }

        public TripSetupViewModel(IMvxNavigationService navigationService, ILogger<TripSetupViewModel> logger)
            : base(navigationService, logger)
        {
            FindTicketsCommand = new MvxAsyncCommand(
                async () =>
                {
                    var result = await navigationService.Navigate<ChooseTicketsViewModel, ChooseTicketsViewModel.ChooseTicketsParameter, ViewModelResult>(
                        new ChooseTicketsViewModel.ChooseTicketsParameter(
                            StartDate ?? new DateTime(2021, 10, 28),
                            EndDate ?? new DateTime(2021, 11, 4)));

                    if (result?.CloseRequested == true)
                    {
                        await Task.Delay(200);
                        await NavigationService.Close(this, new ViewModelResult(true));
                    }
                });

            AddFriendCommand = new MvxAsyncCommand(() => Task.FromResult(true));
            
            NavigateBackCommand = new MvxCommand(() => navigationService.Close(this, new ViewModelResult()));
        }

        public override IMvxCommand NavigateBackCommand { get; }

        public override void OnHardwareBackPressed()
        {
            NavigationService.Close(this, new ViewModelResult());
        }

        public DateTime? StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }
    }
}
