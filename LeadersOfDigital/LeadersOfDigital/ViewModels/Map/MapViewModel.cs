using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business;
using Business.Definitions.Models;
using Business.Definitions.Models.GooglePlacesApi;
using Business.Definitions.Requests;
using Business.Definitions.Responses;
using DebounceThrottle;
using LeadersOfDigital.Definitions;
using LeadersOfDigital.Definitions.Exceptions;
using LeadersOfDigital.ViewModels.Map;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Xamarin.Essentials;

namespace LeadersOfDigital.ViewModels
{
    public class MapViewModel : PageViewModel
    {
        private readonly IGoogleMapsApiService _googleMapsApiService;
        private readonly DebounceDispatcher _debounceDispatcher;
        private string _searchText;
        private GooglePlacesResponse _placesResponse;
        private Place _selectedDestination;
        private bool _isMyLocationEnabled;

        public MapViewModel(
            IMvxNavigationService navigationService,
            IGoogleMapsApiService googleMapsApiService,
            ILogger<MapViewModel> logger)
            : base(navigationService, logger)
        {
            _googleMapsApiService = googleMapsApiService;

            SearchCommand = new MvxCommand<string>(
                async searchText => await _debounceDispatcher.DebounceAsync(SearchAsync));

            ItemTapCommand = new MvxCommand<MapSearchResultItemViewModel>(
                item => SelectedDestination = item.Place);

            SelectDestinationCommand = new MvxCommand<Position>(
                async position =>
                {
                    var destination = new Place
                    {
                        Geometry = new ()
                        {
                            Location = position,
                        },
                    };

                    await PerformAsync(new ViewModelHandledAction(
                            async () =>
                            {
                                State = PageStateType.Loading;

                                var placemarks = (await Geocoding.GetPlacemarksAsync(position.Lat, position.Lng))?.ToList();

                                if (placemarks?.FirstOrDefault() is not { } firstPlacemark)
                                {
                                    destination.FormattedAddress = "Неизвестная локация";

                                    throw new HumanReadableException("Не удалось получить информацию о локации");
                                }

                                var addressStr = !string.IsNullOrEmpty(firstPlacemark.Thoroughfare) ? firstPlacemark.Thoroughfare : null;

                                if (addressStr != null && !string.IsNullOrEmpty(firstPlacemark.SubThoroughfare))
                                {
                                    addressStr += $", {firstPlacemark.SubThoroughfare}";
                                }

                                destination.Name = firstPlacemark.FeatureName;
                                destination.FormattedAddress = addressStr;

                                SelectedDestination = destination;
                            })
                        .SetCommonErrorMessage("Не удалось получить информацию о локации"));

                    State = PageStateType.Content;
                });

            _debounceDispatcher = new DebounceDispatcher(500);
            SearchResults = new MvxObservableCollection<MapSearchResultItemViewModel>();
        }

        public string SearchText
        {
            get => _searchText;
            set => _searchText = value;
        }

        public Place SelectedDestination
        {
            get => _selectedDestination;
            set => SetProperty(ref _selectedDestination, value);
        }

        public IMvxCommand<string> SearchCommand { get; }

        public IMvxCommand<MapSearchResultItemViewModel> ItemTapCommand { get; }

        public IMvxCommand<Position> SelectDestinationCommand { get; }

        public MvxObservableCollection<MapSearchResultItemViewModel> SearchResults { get; }

        public bool IsMyLocationEnabled
        {
            get => _isMyLocationEnabled;
            set => SetProperty(ref _isMyLocationEnabled, value);
        }

        public override void ViewCreated()
        {
            base.ViewCreated();

            Task.Run(() =>
                InvokeOnMainThread(async () =>
                    IsMyLocationEnabled = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>() == PermissionStatus.Granted ||
                                          await Permissions.RequestAsync<Permissions.LocationWhenInUse>() == PermissionStatus.Granted));
        }

        private Task SearchAsync() =>
            PerformAsync(new ViewModelHandledAction(async () =>
            {
                State = PageStateType.Loading;

                SearchResults.Clear();
                await RaisePropertyChanged(nameof(SearchResults));

                _placesResponse = null;

                if (string.IsNullOrEmpty(_searchText))
                {
                    State = PageStateType.Content;

                    return;
                }

                _placesResponse = await _googleMapsApiService.GetPlacesAsync(new GoogleApiPlacesRequest { Query = _searchText }, CancellationToken);

                SearchResults.AddRange(
                    _placesResponse?
                        .Results?
                        .Where(x => x != null)
                        .Select(x => new MapSearchResultItemViewModel(x)
                        {
                            Distance = 1000,
                        }));

                await RaisePropertyChanged(nameof(SearchResults));

                State = PageStateType.Content;
            }));
    }
}
