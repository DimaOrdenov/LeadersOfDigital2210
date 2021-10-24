using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels.Results
{
    public class RecommendedRouteViewModel : PageViewModel
    {
        public IMvxAsyncCommand NextStepCommand { get; }

        public RecommendedRouteViewModel(IMvxNavigationService navigationService, ILogger<RecommendedRouteViewModel> logger)
            : base(navigationService, logger)
        {
        }
    }
}