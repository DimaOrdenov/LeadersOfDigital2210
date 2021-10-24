using MvvmCross.ViewModels;

namespace LeadersOfDigital.Definitions
{
    public class AppStorage : MvxNotifyPropertyChanged
    {
        private Trip _plannedTrip;

        public Trip PlannedTrip
        {
            get => _plannedTrip;
            set => SetProperty(ref _plannedTrip, value);
        }
    }
}
