using LeadersOfDigital.ViewModels.Setup;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels.Main
{
    public class MainViewModel : PageViewModel
    {
        public IMvxAsyncCommand StartPlanningCommand { get; }

        public MainViewModel(IMvxNavigationService navigationService, ILogger<MainViewModel> logger)
            : base(navigationService, logger)
        {
            StartPlanningCommand = new MvxAsyncCommand(() => navigationService.Navigate<TripSetupViewModel>());
        }
    }
}