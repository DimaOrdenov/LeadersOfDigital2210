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
            FindTicketsCommand = new MvxAsyncCommand(() => navigationService.Navigate<MapViewModel>());
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