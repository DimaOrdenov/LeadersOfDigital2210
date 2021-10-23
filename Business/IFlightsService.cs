using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LeadersOfDigital.DataModels.Responses.Flights;

namespace Business
{
    public interface IFlightsService
    {
        Task<IEnumerable<FlightsResponse>> GetFlightsAsync(int originId, int destinationId, DateTime date, CancellationToken token);
    }
}
