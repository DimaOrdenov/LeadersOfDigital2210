using LeadersOfDigital.DataModels.Requests.Trips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadersApi.Services.Trips
{
    public interface ITripsService
    {
        Task<IEnumerable<TripResponse>> GetTrips();

        Task<TripResponse> GetTrip(int id);

        Task<TripResponse> CreateTrip(CreateTripRequest request);
        Task<TripResponse> UpdateTrip(UpdateTripRequest request);
    }
}

