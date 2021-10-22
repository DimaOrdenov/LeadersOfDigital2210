using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels
{
    public class MainViewModel : PageViewModel
    {
        public MainViewModel(IMvxNavigationService navigationService, ILogger<MainViewModel> logger)
            : base(navigationService, logger)
        {
        }
    }
}
