using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels
{
    public class AuthorizationViewModel : PageViewModel
    {
        public AuthorizationViewModel(IMvxNavigationService navigationService, ILogger<AuthorizationViewModel> logger)
            : base(navigationService, logger)
        {
        }
    }
}
