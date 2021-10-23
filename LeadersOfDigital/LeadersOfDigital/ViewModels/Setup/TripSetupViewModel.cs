using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels.Setup
{
    public class TripSetupViewModel : PageViewModel
    {
        private DateTime? _startDate;
        private DateTime? _endDate;

        public IMvxAsyncCommand AddFriendCommand { get; }

        public IMvxAsyncCommand FindTicketsCommand { get; }

        public TripSetupViewModel(IMvxNavigationService navigationService, ILogger<TripSetupViewModel> logger)
            : base(navigationService, logger)
        {
            FindTicketsCommand = new MvxAsyncCommand(() =>
                navigationService.Navigate<ChooseTicketsViewModel, ChooseTicketsViewModel.ChooseTicketsParameter>(
                    new ChooseTicketsViewModel.ChooseTicketsParameter(
                        StartDate ?? new DateTime(2021, 10, 28),
                        EndDate ?? new DateTime(2021, 11, 4))));

            AddFriendCommand = new MvxAsyncCommand(() => Task.FromResult(true));
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
