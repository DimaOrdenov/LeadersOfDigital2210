using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.Definitions.Requests;
using Business.Definitions.Responses;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Business
{
    public class GoogleMapsApiService : BaseService, IGoogleMapsApiService
    {
        private readonly string _googleApisKey;

        public GoogleMapsApiService(
            IRestClient client,
            ILogger logger,
            string googleApisKey)
            : base(client, logger)
        {
            _googleApisKey = googleApisKey ?? throw new ArgumentNullException(nameof(googleApisKey), "Google APIs couldn't be called w/o Google API KEY");
        }

        public Task<GoogleDirectionsResponse> GetDirectionsAsync(GoogleApiDirectionsRequest data, CancellationToken token)
        {
            IRestRequest request = new RestRequest("api/directions/json", Method.GET);

            request.AddParameter("mode", data.TravelMode);
            request.AddParameter("origin", $"{data.Origin.Invoke().Lat},{data.Origin.Invoke().Lng}");
            request.AddParameter("destination", $"{data.Destination.Invoke().Lat},{data.Destination.Invoke().Lng}");

            if (data.Waypoints?.FirstOrDefault() is { } waypoint)
            {
                request.AddParameter("waypoints", $"{waypoint.Lat},{waypoint.Lng}");
            }

            request.AddParameter("alternatives", data.IsProvideAlternatives.ToString());
            request.AddParameter("key", _googleApisKey);

            return ExecuteAsync<GoogleDirectionsResponse>(request, token);
        }

        public Task<GooglePlacesResponse> GetPlacesAsync(GoogleApiPlacesRequest data, CancellationToken token)
        {
            IRestRequest request = new RestRequest("api/place/textsearch/json", Method.GET);

            request.AddParameter("query", data.Query);
            request.AddParameter("language", "ru");
            request.AddParameter("key", _googleApisKey);

            return ExecuteAsync<GooglePlacesResponse>(request, token);
        }
    }
}
