using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business;
using Business.Definitions.Models;
using Business.Definitions.Models.GooglePlacesApi;
using Business.Definitions.Requests;
using Business.Definitions.Responses;
using DebounceThrottle;
using LeadersOfDigital.Android.Definitions.Types;
using LeadersOfDigital.Android.ViewModels.Map;
using LeadersOfDigital.Definitions;
using LeadersOfDigital.Definitions.Exceptions;
using LeadersOfDigital.ViewModels.Map;
using LeadersOfDigital.ViewModels.Setup;
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
        private GoogleDirectionsResponse _googleDirectionsResponse;
        private Position _myPosition;

        public MapViewModel(
            IMvxNavigationService navigationService,
            IGoogleMapsApiService googleMapsApiService,
            ILogger<MapViewModel> logger)
            : base(navigationService, logger)
        {
            _googleMapsApiService = googleMapsApiService;

            SearchCommand = new MvxCommand<string>(
                async searchText => await _debounceDispatcher.DebounceAsync(SearchAsync));

            SetupTripCommand = new MvxAsyncCommand<string>(
                async searchText =>
                {
                    if (SelectedDestination == null)
                    {
                        return;
                    }

                    await navigationService.Navigate<TripSetupViewModel>();
                });

            ItemTapCommand = new MvxCommand<MapSearchResultItemViewModel>(
                async item =>
                {
                    SelectedDestination = item.Place;
                    await ClearSearchResultsAsync();
                });

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

                                var geocodePlaces = await _googleMapsApiService.GetGeocodeAsync(position, CancellationToken);

                                if (geocodePlaces?.Results?.FirstOrDefault(x => x.Geometry?.LocationType == "GEOMETRIC_CENTER") is not { } firstPlace)
                                {
                                    throw new HumanReadableException("Не удалось получить информацию о локации");
                                }

                                destination.Name = firstPlace.AddressComponents?.FirstOrDefault(x => x.Types.Contains("administrative_area_level_2"))?.LongName;
                                destination.FormattedAddress = $"{firstPlace.AddressComponents?.FirstOrDefault(x => x.Types.Contains("administrative_area_level_1"))?.LongName}, " +
                                                               $"{firstPlace.AddressComponents?.FirstOrDefault(x => x.Types.Contains("country"))?.LongName}";

                                SelectedDestination = destination;
                            })
                        .SetCommonErrorMessage("Не удалось получить информацию о локации")
                        .SetIsWithInteraction(true));

                    State = PageStateType.Content;
                });

            GetDirectionsCommand = new MvxCommand<GoogleApiDirectionsRequest>(
                async request =>
                {
                    DirectionsResults.Clear();

                    await PerformAsync(new ViewModelHandledAction(
                            async () =>
                            {
                                var directionsResponse = await _googleMapsApiService.GetDirectionsAsync(
                                    request,
                                    CancellationToken);

                                if (directionsResponse?.Routes?.Any() != true)
                                {
                                    throw new HumanReadableException("Не удалось получить маршруты");
                                }

                                DirectionsResults.AddRange(directionsResponse.Routes.Select(x => new RouteItemViewModel(x)));

                                await RaisePropertyChanged(nameof(DirectionsResults));
                            })
                        .SetIsWithInteraction(true)
                        .SetCommonErrorMessage("Не удалось получить маршруты"));
                });

            _debounceDispatcher = new DebounceDispatcher(500);
            SearchResults = new MvxObservableCollection<MapSearchResultItemViewModel>();
            DirectionsResults = new MvxObservableCollection<RouteItemViewModel>();
        }

        public string SearchText
        {
            get => _searchText;
            set => _searchText = value;
        }

        public Place SelectedDestination
        {
            get => _selectedDestination;
            set
            {
                if (!SetProperty(ref _selectedDestination, value))
                {
                    return;
                }

                DirectionsResults.Clear();
                RaisePropertyChanged(nameof(DirectionsResults));
            }
        }

        public IMvxCommand<string> SearchCommand { get; }
        public IMvxAsyncCommand<string> SetupTripCommand { get; }
        public IMvxCommand<MapSearchResultItemViewModel> ItemTapCommand { get; }

        public IMvxCommand<Position> SelectDestinationCommand { get; }

        public IMvxCommand<GoogleApiDirectionsRequest> GetDirectionsCommand { get; }

        public MvxObservableCollection<MapSearchResultItemViewModel> SearchResults { get; }

        public MvxObservableCollection<RouteItemViewModel> DirectionsResults { get; }

        public bool IsMyLocationEnabled
        {
            get => _isMyLocationEnabled;
            set => SetProperty(ref _isMyLocationEnabled, value);
        }

        public Position MyPosition
        {
            get => _myPosition;
            set => SetProperty(ref _myPosition, value);
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

                await ClearSearchResultsAsync();

                _placesResponse = null;

                if (string.IsNullOrEmpty(_searchText))
                {
                    State = PageStateType.Content;

                    return;
                }

                _placesResponse = await _googleMapsApiService.GetPlacesAsync(_searchText, CancellationToken);

                SearchResults.AddRange(
                    _placesResponse?
                        .Results?
                        .Where(x => x != null)
                        .Select(x => new MapSearchResultItemViewModel(x)
                        {
                            Distance = CalculateDistanceBetweenPositions(MyPosition, x.Geometry.Location),
                        }));

                await RaisePropertyChanged(nameof(SearchResults));

                State = PageStateType.Content;
            }));

        private async Task ClearSearchResultsAsync()
        {
            SearchResults.Clear();
            await RaisePropertyChanged(nameof(SearchResults));
        }

        private double CalculateDistanceBetweenPositions(Position from, Position to)
        {
            var d1 = from.Lat * (Math.PI / 180.0);
            var num1 = from.Lng * (Math.PI / 180.0);
            var d2 = to.Lat * (Math.PI / 180.0);
            var num2 = to.Lng * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376.5 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}
