using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using Business.Definitions.Models;
using Business.Definitions.Requests;
using Definitions.Interactions;
using Google.Android.Material.BottomSheet;
using Java.Lang;
using LeadersOfDigital.Android.Adapters;
using LeadersOfDigital.Android.Definitions.Types;
using LeadersOfDigital.Android.Helpers;
using LeadersOfDigital.Android.Listeners;
using LeadersOfDigital.Converters;
using LeadersOfDigital.ViewModels;
using LeadersOfDigital.ViewModels.Map;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.DroidX.RecyclerView.ItemTemplates;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace LeadersOfDigital.Android.Activities.Map
{
    [MvxActivityPresentation]
    [Activity]
    public partial class MapActivity : MvxActivity<MapViewModel>, IOnMapReadyCallback, GoogleMap.IOnPolylineClickListener, GoogleMap.IOnMapLongClickListener, GoogleMap.IOnMapClickListener,
        View.IOnClickListener, TextView.IOnEditorActionListener, View.IOnTouchListener
    {
        private readonly LocationListener _locationListener;
        private readonly IList<Polyline> _mapPolylines;

        private LocationManager _locationManager;

        private SupportMapFragment _map;
        private GoogleMap _googleMap;
        private EditText _searchBar;
        private MvxRecyclerView _searchResultsList;
        private TextView _chosenDestinationTitle;
        private TextView _chosenDestinationAddress;
        private ImageButton _closeBottomSheetButton;
        private (ConstraintLayout container, ImageView icon, TextView title, TextView price)[] _transportTabs;
        private Button _buildRoute;
        private ConstraintLayout _bottomSheet;
        private BottomSheetBehavior _bottomSheetBehaviour;
        private Marker _currentLocationMarker;
        private Marker _mapMarker;
        private ImageButton _zoomIn;
        private ImageButton _zoomOut;
        private ImageButton _myLocation;
        private readonly int _defaultZoom = 6;
        private readonly int _defaultPolylinesPadding = 400;

        public MapActivity()
        {
            _locationListener = new LocationListener();
            _locationListener.LocationChanged += (_, location) =>
            {
                ViewModel.MyPosition = location.ToPosition();

                var latlng = ViewModel.MyPosition.ToLatLng();

                if (_currentLocationMarker == null)
                {
                    _googleMap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(latlng, _defaultZoom));
                }

                _currentLocationMarker ??= _googleMap.AddMarker(new MarkerOptions()
                    .SetPosition(latlng)
                    .SetTitle("Me"));

                _currentLocationMarker.Position = latlng;
            };

            _mapPolylines = new List<Polyline>();
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _googleMap = googleMap;

            googleMap.UiSettings.CompassEnabled = false;
            googleMap.UiSettings.ZoomControlsEnabled = false;
            googleMap.UiSettings.MyLocationButtonEnabled = false;

            _googleMap.SetOnMapClickListener(this);
            _googleMap.SetOnMapLongClickListener(this);
            _googleMap.SetOnPolylineClickListener(this);

            _mapMarker ??= CreateMarkerIfNotExists();

            var set = CreateBindingSet();

            set.Bind(_googleMap)
                .For(x => x.MyLocationEnabled)
                .To(vm => vm.IsMyLocationEnabled);

            set.Apply();

            this.ShowToast("Определяем ваше местоположение", ToastLength.Short);
        }

        public void OnClick(View v)
        {
            var origin = ViewModel.MyPosition?.ToLatLng();
            var destination = ViewModel.SelectedDestination?.Geometry?.Location?.ToLatLng();

            switch (v.Id)
            {
                case Resource.Id.map_bottom_sheet_close:
                    ViewModel.SelectedDestination = null;

                    break;
                case Resource.Id.map_bottom_sheet_airplane:
                    ActivateTransportTab(0);
                    DeactivateTransportTab(1);
                    DeactivateTransportTab(2);

                    TryRemovePolylines();

                    AddPolyline(new[]
                    {
                        origin,
                        destination,
                    });

                    _googleMap.AnimateCamera(CameraUpdateFactory.NewLatLngBounds(
                        new LatLngBounds.Builder()
                            .Include(origin)
                            .Include(destination)
                            .Build(),
                        _defaultPolylinesPadding));

                    break;
                case Resource.Id.map_bottom_sheet_train:
                    ActivateTransportTab(1);
                    DeactivateTransportTab(0);
                    DeactivateTransportTab(2);

                    TryRemovePolylines();

                    AddPolyline(new[]
                    {
                        origin,
                        destination,
                    });

                    _googleMap.AnimateCamera(CameraUpdateFactory.NewLatLngBounds(
                        new LatLngBounds.Builder()
                            .Include(origin)
                            .Include(destination)
                            .Build(),
                        _defaultPolylinesPadding));

                    break;
                case Resource.Id.map_bottom_sheet_car:
                    ActivateTransportTab(2);
                    DeactivateTransportTab(0);
                    DeactivateTransportTab(1);

                    TryRemovePolylines();

                    ViewModel.GetDirectionsCommand.Execute(
                        new GoogleApiDirectionsRequest(
                            () => ViewModel.MyPosition,
                            () => ViewModel.SelectedDestination.Geometry.Location));

                    break;
                case Resource.Id.map_zoom_in:
                    _googleMap.AnimateCamera(CameraUpdateFactory.ZoomIn());

                    break;
                case Resource.Id.map_zoom_out:
                    _googleMap.AnimateCamera(CameraUpdateFactory.ZoomOut());

                    break;
                case Resource.Id.map_my_location:
                    if (ViewModel.MyPosition == null)
                    {
                        this.ShowToast("Couldn't determine current location", ToastLength.Short);

                        return;
                    }

                    _googleMap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(ViewModel.MyPosition.ToLatLng(), _defaultZoom));

                    break;
            }
        }

        public bool OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
        {
            if (actionId == ImeAction.Search)
            {
                ViewModel.SearchCommand.Execute(v.Text);

                return true;
            }

            return false;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            if (e.Action == MotionEventActions.Up &&
                v is EditText searchBar)
            {
                var end = searchBar.Right;

                if (e.RawX >= end - searchBar.CompoundPaddingEnd)
                {
                    searchBar.Text = null;

                    return true;
                }
            }

            return false;
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
                (
                    FindViewById<ConstraintLayout>(Resource.Id.map_bottom_sheet_airplane),
                    FindViewById<ImageView>(Resource.Id.map_bottom_sheet_airplane_icon),
                    FindViewById<TextView>(Resource.Id.map_bottom_sheet_airplane_title),
                    FindViewById<TextView>(Resource.Id.map_bottom_sheet_airplane_price)
                ),
                (
                    FindViewById<ConstraintLayout>(Resource.Id.map_bottom_sheet_train),
                    FindViewById<ImageView>(Resource.Id.map_bottom_sheet_train_icon),
                    FindViewById<TextView>(Resource.Id.map_bottom_sheet_train_title),
                    FindViewById<TextView>(Resource.Id.map_bottom_sheet_train_price)
                ),
                (
                    FindViewById<ConstraintLayout>(Resource.Id.map_bottom_sheet_car),
                    FindViewById<ImageView>(Resource.Id.map_bottom_sheet_car_icon),
                    FindViewById<TextView>(Resource.Id.map_bottom_sheet_car_title),
                    FindViewById<TextView>(Resource.Id.map_bottom_sheet_car_price)
                ),
            };
            _buildRoute = FindViewById<Button>(Resource.Id.map_bottom_sheet_build_route);
            _bottomSheet = FindViewById<ConstraintLayout>(Resource.Id.map_bottom_sheet);
            _bottomSheetBehaviour = BottomSheetBehavior.From(_bottomSheet);
            _zoomIn = FindViewById<ImageButton>(Resource.Id.map_zoom_in);
            _zoomOut = FindViewById<ImageButton>(Resource.Id.map_zoom_out);
            _myLocation = FindViewById<ImageButton>(Resource.Id.map_my_location);

            _bottomSheetBehaviour.Hideable = true;
            _bottomSheetBehaviour.Draggable = false;
            _bottomSheetBehaviour.State = BottomSheetBehavior.StateHidden;

            _closeBottomSheetButton.SetOnClickListener(this);

            var adapter = new MapSearchResultsAdapter((IMvxAndroidBindingContext)BindingContext)
            {
                ItemTemplateSelector = new MvxDefaultTemplateSelector(Resource.Layout.MapSearchResultItemView),
            };

            _searchResultsList.SetAdapter(adapter);
            adapter.ItemClick = new MvxCommand<MapSearchResultItemViewModel>(
                item =>
                {
                    ViewModel.ItemTapCommand.Execute(item);

                    _view.HideKeyboard(this);

                    if (item.Place?.Geometry?.Location is { } location)
                    {
                        _googleMap?.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(location.ToLatLng(), _defaultZoom));
                    }
                });

            _map.GetMapAsync(this);

            _locationManager = LocationManager.FromContext(ApplicationContext);

            _searchBar.TextChanged += (_, e) => ViewModel.SearchCommand.Execute(e.Text?.ToString());
            _searchBar.SetOnTouchListener(this);

            _searchBar.SetOnEditorActionListener(this);

            foreach (var transportTab in _transportTabs)
            {
                transportTab.container.SetOnClickListener(this);
            }

            _zoomIn.SetOnClickListener(this);
            _zoomOut.SetOnClickListener(this);
            _myLocation.SetOnClickListener(this);

            AddBindings(adapter);

            this.AddLoadingStateActivityBindings<MapActivity, MapViewModel>();
        }

        protected override void OnResume()
        {
            base.OnResume();

            try
            {
                foreach (var provider in _locationManager.GetProviders(true).Where(x => !string.IsNullOrEmpty(x)))
                {
                    _locationManager.RequestLocationUpdates(provider, 0, 0, _locationListener);
                }
            }
            catch (Exception ex)
            {
                ViewModel.Logger.LogWarning(ex, "Couldn't start location update");

                this.ShowToast("Couldn't start location update", ToastLength.Short);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();

            _locationManager.RemoveUpdates(_locationListener);
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
                    .SetPosition(point ?? new LatLng(0, 0));

                _mapMarker = _googleMap.AddMarker(markerOpt);

                var set = CreateBindingSet();

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

        private void ActivateTransportTab(int index)
        {
            _transportTabs[index].container.SetBackgroundResource(Resource.Drawable.ripple_rounded_accent);
            _transportTabs[index].icon.ImageTintList = Resources.GetColorStateList(Resource.Color.colorWhite, null);
            _transportTabs[index].title.SetTextColor(Resources.GetColor(Resource.Color.colorWhite, null));
            _transportTabs[index].price.SetTextColor(Resources.GetColor(Resource.Color.colorWhite, null));
        }

        private void DeactivateTransportTab(int index)
        {
            _transportTabs[index].container.SetBackgroundResource(Resource.Drawable.ripple_rounded_white);
            _transportTabs[index].icon.ImageTintList = Resources.GetColorStateList(Resource.Color.colorText, null);
            _transportTabs[index].title.SetTextColor(Resources.GetColor(Resource.Color.colorText, null));
            _transportTabs[index].price.SetTextColor(Resources.GetColor(Resource.Color.colorText, null));
        }

        private void DeactivateTransportTabs()
        {
            DeactivateTransportTab(0);
            DeactivateTransportTab(1);
            DeactivateTransportTab(2);
        }

        private void AddPolyline(LatLng[] polylinePoints)
        {
            var polylineOpts = new PolylineOptions().Add(polylinePoints);

            polylineOpts.Clickable(true);
            polylineOpts.InvokeColor(Resources.GetColor(Resource.Color.colorAccent, null));
            polylineOpts.InvokeWidth(8);

            _mapPolylines.Add(_googleMap.AddPolyline(polylineOpts));
        }

        private void TryRemovePolylines()
        {
            if (_mapPolylines == null)
            {
                return;
            }

            foreach (var mapPolyline in _mapPolylines)
            {
                mapPolyline.Remove();
            }

            _mapPolylines.Clear();
        }
    }
}
