using System.ComponentModel;
using System.Linq;
using Android.Gms.Maps.Model;
using Android.Views;
using Google.Android.Material.BottomSheet;
using LeadersOfDigital.Android.Adapters;

namespace LeadersOfDigital.Android.Activities
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

            set.Bind(adapter)
                .For(x => x.ItemClick)
                .To(vm => vm.ItemTapCommand);

            set.Bind(_chosenDestinationTitle)
                .For(x => x.Text)
                .To(vm => vm.SelectedDestination.Name);

            set.Bind(_chosenDestinationAddress)
                .For(x => x.Text)
                .To(vm => vm.SelectedDestination.FormattedAddress);

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
                        _mapMarker.Position = new LatLng(ViewModel.SelectedDestination.Geometry.Location.Lat, ViewModel.SelectedDestination.Geometry.Location.Lng);
                    }

                    break;
            }
        }
    }
}
