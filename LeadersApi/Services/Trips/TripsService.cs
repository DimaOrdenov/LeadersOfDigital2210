using LeadersOfDigital.DataModels.Requests.Trips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadersApi.Services.Trips
{
    public class TripsService : ITripsService
    {
        public TripsService()
        {

        }

        public Task<TripResponse> CreateTrip(CreateTripRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<TripResponse> GetTrip(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TripResponse>> GetTrips()
        {
            throw new NotImplementedException();
        }

        public Task<TripResponse> UpdateTrip(UpdateTripRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
