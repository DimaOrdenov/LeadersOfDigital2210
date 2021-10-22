using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace LeadersOfDigital.ViewModels.Authorization
{
    public class AuthorizationViewModel : PageViewModel
    {
        public IMvxAsyncCommand AuthorizationCommand { get; }

        public IMvxAsyncCommand SkipAuthCommand { get; }

        public IMvxAsyncCommand AuthViaSocialNetworkCommand { get; }

        public AuthorizationViewModel(IMvxNavigationService navigationService, ILogger<AuthorizationViewModel> logger)
            : base(navigationService, logger)
        {
            AuthorizationCommand = new MvxAsyncCommand(() => GoToMainPageAsync());
            SkipAuthCommand = new MvxAsyncCommand(() => GoToMainPageAsync());
            AuthViaSocialNetworkCommand = new MvxAsyncCommand(() => GoToMainPageAsync());

            Task GoToMainPageAsync() => navigationService.Navigate<MainViewModel>();
        }
    }
}
