using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels.Main
{
    public class FavoritesViewModel : PageViewModel
    {
        public FavoritesViewModel(IMvxNavigationService navigationService, ILogger<FavoritesViewModel> logger)
            : base(navigationService, logger)
        {
        }
    }
}

