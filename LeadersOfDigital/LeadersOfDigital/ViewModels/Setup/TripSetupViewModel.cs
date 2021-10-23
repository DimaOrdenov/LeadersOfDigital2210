using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels.Setup
{
    public class TripSetupViewModel : PageViewModel
    {
        public IMvxAsyncCommand AddFriendCommand { get; }
        public IMvxAsyncCommand FindTicketsCommand { get; }

        public TripSetupViewModel(IMvxNavigationService navigationService, ILogger<TripSetupViewModel> logger)
            : base(navigationService, logger)
        {
            FindTicketsCommand = new MvxAsyncCommand(() => navigationService.Navigate<MapViewModel>());
            AddFriendCommand = new MvxAsyncCommand(() => Task.FromResult(true));
        }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}