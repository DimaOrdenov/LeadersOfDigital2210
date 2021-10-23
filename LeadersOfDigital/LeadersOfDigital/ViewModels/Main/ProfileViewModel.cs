using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels.Main
{
    public class ProfileViewModel : PageViewModel
    {
        public ProfileViewModel(IMvxNavigationService navigationService, ILogger<ProfileViewModel> logger)
            : base(navigationService, logger)
        {
        }
    }
}