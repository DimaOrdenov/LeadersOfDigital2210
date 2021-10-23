using Business.Definitions.Models.GooglePlacesApi;
using LeadersOfDigital.Definitions.Attributes;
using LeadersOfDigital.Helpers;
using MvvmCross.ViewModels;

namespace LeadersOfDigital.ViewModels.Map
{
    public class MapSearchResultItemViewModel : MvxNotifyPropertyChanged
    {
        public MapSearchResultItemViewModel(Place place)
        {
            Place = place;

            PlaceType = place.Types?.Contains("airport") == true ?
                PlaceType.Airport :
                place.Types?.Contains("train_station") == true ?
                    PlaceType.Train :
                    PlaceType.Default;

            PlaceImage = PlaceType.GetPropertyAttributeValue<PlaceType, ImageAttribute, string>(x => x.ImageResource);
        }

        public Place Place { get; }

        public double Distance { get; set; }

        public PlaceType PlaceType { get; }

        public string DistanceStr => $"{Distance:F1} км";

        public string PlaceImage { get; }
    }
}
