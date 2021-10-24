using LeadersOfDigital.ViewModels.Results;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels.Setup
{
    public class InterestsSetupViewModel : PageViewModel
    {
        public IMvxAsyncCommand NextStepCommand { get; }

        public InterestsSetupViewModel(IMvxNavigationService navigationService, ILogger<InterestsSetupViewModel> logger)
            : base(navigationService, logger)
        {
            NextStepCommand = new MvxAsyncCommand(() => navigationService.Navigate<RecommendedRouteViewModel>());
        }
    }
}
