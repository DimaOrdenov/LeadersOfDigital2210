using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels
{
    public class MainTabbedViewModel : PageViewModel
    {
        public MainTabbedViewModel(IMvxNavigationService navigationService, ILogger<MainTabbedViewModel> logger)
            : base(navigationService, logger)
        {
        }
    }
}
