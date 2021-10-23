using Business.Definitions.Models.GooglePlacesApi;
using MvvmCross.ViewModels;

namespace LeadersOfDigital.ViewModels.Map
{
    public class MapSearchResultItemViewModel : MvxNotifyPropertyChanged
    {
        public MapSearchResultItemViewModel(Place place)
        {
            Place = place;
        }

        public Place Place { get; }

        public double Distance { get; set; }

        public PlaceType PlaceType { get; set; }

        public string DistanceStr => $"{Distance} км";
    }
}
