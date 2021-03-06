using System.Reflection;
using Business;
using LeadersOfDigital.ViewModels.Authorization;
using LeadersOfDigital.ViewModels.Main;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using RestSharp;
using LeadersOfDigital.Android.Helpers;
using LeadersOfDigital.Definitions;
using LeadersOfDigital.Android.Services;
using LeadersOfDigital.Services;

namespace LeadersOfDigital
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            CreatableTypes(Assembly.GetAssembly(typeof(IFlightsService)))
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IRestClient>(() =>
                new RestClient("https://leadersapi.azurewebsites.net/"));

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IGoogleMapsApiService>(
                () => new GoogleMapsApiService(
                    new RestClient("https://maps.googleapis.com/maps/"),
                    Mvx.IoCProvider.Resolve<ILogger<GoogleMapsApiService>>(),
                    Secrets.GoogleApisKey
                ));

            Mvx.IoCProvider.RegisterSingleton(new AppStorage());

            Mvx.IoCProvider.RegisterType<IDialogService, DialogService>();
            Mvx.IoCProvider.RegisterType<MainViewModel>();
            Mvx.IoCProvider.RegisterType<FavoritesViewModel>();
            Mvx.IoCProvider.RegisterType<PopularViewModel>();
            Mvx.IoCProvider.RegisterType<ProfileViewModel>();

            RegisterAppStart<AuthorizationViewModel>();
        }
    }
}
