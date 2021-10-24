using System;
using LeadersOfDigital.DataModels.Responses.Flights;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace LeadersOfDigital.ViewModels.Setup
{
    public class TicketItemViewModel : MvxNotifyPropertyChanged
    {
        public TicketItemViewModel(FlightsResponse flightsResponse)
        {
            FlightsResponse = flightsResponse;

            Route = $"{flightsResponse.Origin_airport} - {flightsResponse.Destination_airport}";
            TimeRange = $"{flightsResponse.Departure_at:HH:mm} - {flightsResponse.Departure_at.AddMinutes(flightsResponse.Duration):HH:mm}";
            Duration = string.Format("{0:%h} ч {0:%m}  мин", TimeSpan.FromMinutes(flightsResponse.Duration));
        }

        public IMvxCommand BuyCommand { get; set; }

        public FlightsResponse FlightsResponse { get; }

        public string Route { get; }

        public string TimeRange { get; }

        public string Duration { get; }
    }
}
