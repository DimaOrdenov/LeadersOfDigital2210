using Business;
using LeadersOfDigital.ViewModels.Authorization;
using LeadersOfDigital.ViewModels.Main;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using RestSharp;
using LeadersOfDigital.Android.Helpers;
using LeadersOfDigital.ViewModels;
using MainViewModel = LeadersOfDigital.ViewModels.Main.MainViewModel;

namespace LeadersOfDigital
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IGoogleMapsApiService>(
                () => new GoogleMapsApiService(
                    new RestClient("https://maps.googleapis.com/maps/"),
                    Mvx.IoCProvider.Resolve<ILogger<GoogleMapsApiService>>(),
                    Secrets.GoogleApisKey
                ));

            Mvx.IoCProvider.RegisterType<MainViewModel>();
            Mvx.IoCProvider.RegisterType<FavoritesViewModel>();
            Mvx.IoCProvider.RegisterType<PopularViewModel>();
            Mvx.IoCProvider.RegisterType<ProfileViewModel>();

            RegisterAppStart<MapViewModel>();
        }
    }
}
