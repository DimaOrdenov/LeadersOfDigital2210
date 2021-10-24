using System;
using System.ComponentModel;
using System.Linq;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Views;
using Android.Widget;
using Google.Android.Material.BottomSheet;
using LeadersOfDigital.Android.Adapters;
using LeadersOfDigital.Android.Helpers;
using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Android.Binding;

namespace LeadersOfDigital.Android.Activities.Map
{
    public partial class MapActivity
    {
        private void AddBindings(MapSearchResultsAdapter adapter)
        {
            var set = CreateBindingSet();

            set.Bind(_searchBar)
                .For(x => x.Text)
                .To(vm => vm.SearchText);

            set.Bind(adapter)
                .For(x => x.ItemsSource)
                .To(vm => vm.SearchResults);

            set.Bind(_chosenDestinationTitle)
                .For(x => x.Text)
                .To(vm => vm.SelectedDestination.Name);

            set.Bind(_chosenDestinationAddress)
                .For(x => x.Text)
                .To(vm => vm.SelectedDestination.FormattedAddress);

            set.Bind(this)
                .For(x => x.CommonExceptionInteraction)
                .To(vm => vm.CommonExceptionInteraction)
                .OneWay();

            set.Bind(this)
                .For(x => x.HumanReadableExceptionInteraction)
                .To(vm => vm.HumanReadableExceptionInteraction)
                .OneWay();

            set.Bind(_buildRoute)
                .For(x => x.BindClick())
                .To(vm => vm.SetupTripCommand);

            set.Bind(FindViewById<ImageButton>(Resource.Id.back_button))
                .For(x => x.BindClick())
                .To(vm => vm.NavigateBackCommand);

            set.Apply();

            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.SearchResults):
                    _searchResultsList.Visibility = ViewModel.SearchResults?.Any() == true ? ViewStates.Visible : ViewStates.Gone;

                    break;
                case nameof(ViewModel.SelectedDestination):
                    _bottomSheetBehaviour.State = ViewModel.SelectedDestination != null ? BottomSheetBehavior.StateExpanded : BottomSheetBehavior.StateHidden;

                    if (_mapMarker != null &&
                        ViewModel.SelectedDestination != null)
                    {
                        _mapMarker.Position = ViewModel.SelectedDestination.Geometry.Location.ToLatLng();
                    }

                    DeactivateTransportTabs();

                    break;
                case nameof(ViewModel.DirectionsResults):
                    if (!ViewModel.DirectionsResults.Any())
                    {
                        TryRemovePolylines();

                        break;
                    }

                    var boundsBuilder = new LatLngBounds.Builder();

                    foreach (var route in ViewModel.DirectionsResults)
                    {
                        var polylinePoints = route.Points.Select(x => x.ToLatLng()).ToArray();

                        AddPolyline(polylinePoints);

                        foreach (var point in polylinePoints)
                        {
                            boundsBuilder.Include(point);
                        }
                    }

                    try
                    {
                        _googleMap.AnimateCamera(CameraUpdateFactory.NewLatLngBounds(boundsBuilder.Build(), _defaultPolylinesPadding));
                    }
                    catch (Exception ex)
                    {
                        ViewModel.Logger.LogError(ex, "Couldn't animate camera");
                    }

                    break;
            }
        }
    }
}
