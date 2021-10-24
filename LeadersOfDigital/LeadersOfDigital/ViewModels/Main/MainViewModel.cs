using System.Threading.Tasks;
using Google.Android.Material.Snackbar;
using LeadersOfDigital.Definitions;
using LeadersOfDigital.Services;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Xamarin.Essentials;

namespace LeadersOfDigital.ViewModels.Main
{
    public class MainViewModel : PageViewModel
    {
        public MainViewModel(
            AppStorage appStorage,
            IMvxNavigationService navigationService,
            IDialogService dialogService,
            ILogger<MainViewModel> logger)
            : base(navigationService, logger)
        {
            AppStorage = appStorage;

            StartPlanningCommand = new MvxAsyncCommand(async () =>
            {
                if (await CheckAndRequestLocationPermissionIfNeeded() != PermissionStatus.Granted)
                {
                    dialogService.ShowToast("Для работы с картой необходимо предоставить доступ к геолокации");
                    AppInfo.ShowSettingsUI();
                    return;
                }

                await navigationService.Navigate<MapViewModel>();
            });
        }

        public IMvxAsyncCommand StartPlanningCommand { get; }

        public AppStorage AppStorage { get; }

        private async Task<PermissionStatus> CheckAndRequestLocationPermissionIfNeeded()
        {
            var permissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (permissionStatus != PermissionStatus.Granted)
            {
                permissionStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            return permissionStatus;
        }
    }
}
