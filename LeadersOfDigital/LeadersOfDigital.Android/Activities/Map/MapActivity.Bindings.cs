using System.ComponentModel;
using System.Linq;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Views;
using Google.Android.Material.BottomSheet;
using LeadersOfDigital.Android.Adapters;
using LeadersOfDigital.Android.Helpers;

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

                    if (ViewModel.SelectedDestination == null)
                    {
                        DeactivateTransportTabs();
                    }

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

                    _googleMap.AnimateCamera(CameraUpdateFactory.NewLatLngBounds(boundsBuilder.Build(), 400));

                    break;
            }
        }
    }
}
