using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels
{
    public class MapViewModel : PageViewModel
    {
        public MapViewModel(IMvxNavigationService navigationService, ILogger<MapViewModel> logger)
            : base(navigationService, logger)
        {
        }
    }
}
