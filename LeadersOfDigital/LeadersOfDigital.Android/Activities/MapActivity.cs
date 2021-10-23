using System.ComponentModel;
using System.Linq;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using Google.Android.Material.BottomSheet;
using Java.Lang;
using LeadersOfDigital.Android.Adapters;
using LeadersOfDigital.Android.Listeners;
using LeadersOfDigital.Converters;
using LeadersOfDigital.ViewModels;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.DroidX.RecyclerView.ItemTemplates;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace LeadersOfDigital.Android.Activities
{
    [MvxActivityPresentation]
    [Activity]
    public partial class MapActivity : MvxActivity<MapViewModel>, IOnMapReadyCallback, GoogleMap.IOnPolylineClickListener, GoogleMap.IOnMapLongClickListener, GoogleMap.IOnMapClickListener,
        View.IOnClickListener
    {
        private readonly LocationListener _locationListener;

        private LatLng _currentLocation;

        private SupportMapFragment _map;
        private GoogleMap _googleMap;
        private EditText _searchBar;
        private MvxRecyclerView _searchResultsList;
        private TextView _chosenDestinationTitle;
        private TextView _chosenDestinationAddress;
        private ImageButton _closeBottomSheetButton;
        private ConstraintLayout[] _transportTabs;
        private Button _buildRoute;
        private ConstraintLayout _bottomSheet;
        private BottomSheetBehavior _bottomSheetBehaviour;
        private Marker _currentLocationMarker;
        private Marker _mapMarker;

        public MapActivity()
        {
            _locationListener = new LocationListener();
            _locationListener.LocationChanged += (_, location) =>
            {
                _currentLocation = new LatLng(location.Latitude, location.Longitude);

                _currentLocationMarker ??= _googleMap.AddMarker(new MarkerOptions()
                    .SetPosition(_currentLocation)
                    .SetTitle("Me")
                    // .SetIcon(BitmapDescriptorExtensions.GetBitmapDescriptorFromVector(ApplicationContext, Resource.Drawable.ic_my_location))
                    // .SetRotation(location.Bearing)
                    // .Anchor(0.5f, 0.5f)
                );

                _currentLocationMarker.Position = _currentLocation;
            };

            // _mapPolylines = new List<Polyline>();
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _googleMap = googleMap;

            googleMap.UiSettings.CompassEnabled = false;
            googleMap.UiSettings.ZoomControlsEnabled = false;
            // googleMap.UiSettings.MyLocationButtonEnabled = false;

            _googleMap.SetOnMapClickListener(this);
            _googleMap.SetOnMapLongClickListener(this);
            _googleMap.SetOnPolylineClickListener(this);

            _mapMarker ??= CreateMarkerIfNotExists();

            var set = CreateBindingSet();

            set.Bind(_googleMap)
                .For(x => x.MyLocationEnabled)
                .To(vm => vm.IsMyLocationEnabled);

            set.Apply();
        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.map_bottom_sheet_close:
                    _bottomSheetBehaviour.State = BottomSheetBehavior.StateHidden;

                    break;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MapActivity);

            _map = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map_map);
            _searchBar = FindViewById<EditText>(Resource.Id.map_search_text);
            _searchResultsList = FindViewById<MvxRecyclerView>(Resource.Id.map_search_results);
            _chosenDestinationTitle = FindViewById<TextView>(Resource.Id.map_bottom_sheet_from_city);
            _chosenDestinationAddress = FindViewById<TextView>(Resource.Id.map_bottom_sheet_from_address);
            _closeBottomSheetButton = FindViewById<ImageButton>(Resource.Id.map_bottom_sheet_close);
            _transportTabs = new[]
            {
                FindViewById<ConstraintLayout>(Resource.Id.map_bottom_sheet_airplane),
                FindViewById<ConstraintLayout>(Resource.Id.map_bottom_sheet_train),
                FindViewById<ConstraintLayout>(Resource.Id.map_bottom_sheet_car),
            };
            _buildRoute = FindViewById<Button>(Resource.Id.map_bottom_sheet_build_route);
            _bottomSheet = FindViewById<ConstraintLayout>(Resource.Id.map_bottom_sheet);
            _bottomSheetBehaviour = BottomSheetBehavior.From(_bottomSheet);
            _bottomSheetBehaviour.Hideable = true;
            _bottomSheetBehaviour.Draggable = false;
            _bottomSheetBehaviour.State = BottomSheetBehavior.StateHidden;

            _closeBottomSheetButton.SetOnClickListener(this);

            var adapter = new MapSearchResultsAdapter((IMvxAndroidBindingContext)BindingContext)
            {
                ItemTemplateSelector = new MvxDefaultTemplateSelector(Resource.Layout.MapSearchResultItemView),
            };

            _searchResultsList.SetAdapter(adapter);

            _map.GetMapAsync(this);

            _searchBar.TextChanged += (_, e) => ViewModel.SearchCommand.Execute(e.Text?.ToString());

            AddBindings(adapter);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (ViewModel != null)
            {
                ViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
            }
        }
        
        private Marker CreateMarkerIfNotExists(LatLng point = null)
        {
            if (_mapMarker == null)
            {
                var markerOpt = new MarkerOptions()
                    .SetPosition(point ?? new LatLng(0, 0))
                    // .SetIcon(BitmapDescriptorExtensions.GetBitmapDescriptorFromVector(this, Resource.Drawable.ic_pin_filled))
                    // .Anchor(0.2f, 0.1f)
                    ;

                _mapMarker = _googleMap.AddMarker(markerOpt);

                var set = CreateBindingSet();

                // set.Bind(_mapMarker)
                //     .For(nameof(MarkerPositionTargetBinding))
                //     .To(vm => vm.SelectedDestination.Geometry.Location);
                
                set.Bind(_mapMarker)
                    .For(x => x.Title)
                    .To(vm => vm.SelectedDestination.FormattedAddress);
                
                set.Bind(_mapMarker)
                    .For(x => x.Visible)
                    .To(vm => vm.SelectedDestination)
                    .WithConversion<IsNotNullConverter>();

                set.Apply();
            }

            return _mapMarker;
        }
    }
}
