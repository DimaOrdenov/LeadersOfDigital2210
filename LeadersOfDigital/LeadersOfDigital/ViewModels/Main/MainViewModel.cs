using LeadersOfDigital.Definitions;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels.Main
{
    public class MainViewModel : PageViewModel
    {
        public MainViewModel(
            AppStorage appStorage,
            IMvxNavigationService navigationService,
            ILogger<MainViewModel> logger)
            : base(navigationService, logger)
        {
            AppStorage = appStorage;

            StartPlanningCommand = new MvxAsyncCommand(() => navigationService.Navigate<MapViewModel>());
        }

        public IMvxAsyncCommand StartPlanningCommand { get; }

        public AppStorage AppStorage { get; }
    }
}
