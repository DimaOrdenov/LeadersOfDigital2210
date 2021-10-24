using System.ComponentModel;
using Android.App;
using Android.OS;
using Android.Util;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using Definitions.Interactions;
using Google.Android.Material.Button;
using LeadersOfDigital.Android.Adapters;
using LeadersOfDigital.Android.Helpers;
using LeadersOfDigital.ViewModels.Setup;
using MvvmCross.Commands;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.DroidX.RecyclerView.ItemTemplates;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace LeadersOfDigital.Android.Activities.Setup
{
    [MvxActivityPresentation]
    [Activity]
    public class ChooseTicketsActivity : MvxActivity<ChooseTicketsViewModel>
    {
        private Button _back;
        private TextView _currentPrice;
        private TextView _maxPrice;
        private TextView _tripCities;
        private TextView _tripDates;
        private MaterialButton _firstTripDay;
        private MaterialButton _lastTripDay;
        private MvxRecyclerView _resultsList;
        private ConstraintLayout _nextStep;

        private IExtendedInteraction<BaseInteractionResult> _commonExceptionInteraction;
        private IExtendedInteraction<BaseInteractionResult> _humanReadableExceptionInteraction;

        public IExtendedInteraction<BaseInteractionResult> CommonExceptionInteraction
        {
            get => _commonExceptionInteraction;
            set => this.SetInteraction(ref _commonExceptionInteraction, value);
        }

        public IExtendedInteraction<BaseInteractionResult> HumanReadableExceptionInteraction
        {
            get => _humanReadableExceptionInteraction;
            set => this.SetInteraction(ref _humanReadableExceptionInteraction, value);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ChooseTicketsView);

            _back = FindViewById<Button>(Resource.Id.choose_tickets_back);
            _currentPrice = FindViewById<TextView>(Resource.Id.choose_tickets_budget_current);
            _maxPrice = FindViewById<TextView>(Resource.Id.choose_tickets_budget_max);
            _tripCities = FindViewById<TextView>(Resource.Id.choose_tickets_route_panel_cities);
            _tripDates = FindViewById<TextView>(Resource.Id.choose_tickets_route_panel_dates);
            _firstTripDay = FindViewById<MaterialButton>(Resource.Id.choose_tickets_dates_from);
            _lastTripDay = FindViewById<MaterialButton>(Resource.Id.choose_tickets_dates_to);
            _resultsList = FindViewById<MvxRecyclerView>(Resource.Id.choose_tickets_tickets_list);
            _nextStep = FindViewById<ConstraintLayout>(Resource.Id.choose_tickets_next_step);

            var adapter = new TicketsAdapter((IMvxAndroidBindingContext)BindingContext)
            {
                ItemTemplateSelector = new MvxDefaultTemplateSelector(Resource.Layout.ChooseTicketsListItem),
            };
            _resultsList.SetAdapter(adapter);

            var set = CreateBindingSet();

            set.Bind(_back)
                .For(x => x.BindClick())
                .To(vm => vm.NavigateBackCommand);

            set.Bind(adapter)
                .For(x => x.ItemsSource)
                .To(vm => vm.TicketsResults);

            set.Bind(_nextStep)
                .For(x => x.BindClick())
                .To(vm => vm.NextStepCommand);

            set.Bind(this)
                .For(x => x.CommonExceptionInteraction)
                .To(vm => vm.CommonExceptionInteraction)
                .OneWay();

            set.Bind(this)
                .For(x => x.HumanReadableExceptionInteraction)
                .To(vm => vm.HumanReadableExceptionInteraction)
                .OneWay();

            set.Apply();

            _tripDates.Text = $"{ViewModel.NavigationParameter.DepartsAt:dd.MM} - {ViewModel.NavigationParameter.ArrivesAt:dd.MM}";
            _firstTripDay.Text = ViewModel.NavigationParameter.DepartsAt.ToString("dd.MM.yyyy");
            _lastTripDay.Text = ViewModel.NavigationParameter.ArrivesAt.ToString("dd.MM.yyyy");

            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;

            this.AddLoadingStateActivityBindings<ChooseTicketsActivity, ChooseTicketsViewModel>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (ViewModel != null)
            {
                ViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
            }
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.DepartureTicket):
                    if (ViewModel.DepartureTicket == null)
                    {
                        return;
                    }

                    _firstTripDay.SetIconResource(Resource.Drawable.ic_check_circle);
                    _firstTripDay.SetTextSize(ComplexUnitType.Sp, 18);
                    _lastTripDay.SetTextSize(ComplexUnitType.Sp, 24);

                    break;
                case nameof(ViewModel.ArrivalTicket):
                    if (ViewModel.ArrivalTicket == null)
                    {
                        return;
                    }

                    _lastTripDay.SetIconResource(Resource.Drawable.ic_check_circle);
                    _lastTripDay.SetTextSize(ComplexUnitType.Sp, 18);

                    break;
            }
        }
    }
}
