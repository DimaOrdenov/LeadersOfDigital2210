using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LeadersOfDigital.DataModels.Responses.Flights;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Business
{
    public class FlightsService : BaseService, IFlightsService
    {
        public FlightsService(IRestClient client, ILogger<FlightsService> logger)
            : base(client, logger)
        {
        }

        public Task<IEnumerable<FlightsResponse>> GetFlightsAsync(int originId, int destinationId, DateTime date, CancellationToken token)
        {
            var request = new RestRequest("flights")
                .AddQueryParameter("origin", originId.ToString())
                .AddQueryParameter("destination", destinationId.ToString())
                .AddQueryParameter("departure", date.ToString("yyyy-MM-dd"));

            return ExecuteAsync<IEnumerable<FlightsResponse>>(request, token);
        }
    }
}
