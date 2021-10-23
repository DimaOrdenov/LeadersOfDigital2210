using Business;
using LeadersOfDigital.ViewModels;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.IoC;
using LeadersOfDigital.ViewModels.Authorization;
using MvvmCross.ViewModels;
using RestSharp;
using LeadersOfDigital.Android.Helpers;

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

            RegisterAppStart<AuthorizationViewModel>();
        }
    }
}
