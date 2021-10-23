using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels.Main
{
    public class PopularViewModel : PageViewModel
    {
        public PopularViewModel(IMvxNavigationService navigationService, ILogger<PopularViewModel> logger)
            : base(navigationService, logger)
        {
        }
    }
}