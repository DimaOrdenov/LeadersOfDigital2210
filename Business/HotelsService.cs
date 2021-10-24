using System;
using System.Threading;
using System.Threading.Tasks;
using LeadersOfDigital.DataModels.Responses.Hotels;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Business
{
    public class HotelsService : BaseService, IHotelsService
    {
        public HotelsService(IRestClient client, ILogger<HotelsService> logger)
            : base(client, logger)
        {
        }

        public Task<HotelsResponse> GetSuggestedHotelAsync(int maxBudget, int currentBudget, int city, DateTime from, DateTime to, CancellationToken token)
        {
            var request = new RestRequest("hotels/suggested")
                .AddQueryParameter("maxBudget", maxBudget.ToString())
                .AddQueryParameter("currentBudget", currentBudget.ToString())
                .AddQueryParameter("city", city.ToString())
                .AddQueryParameter("from", from.ToString("yyyy-MM-dd"))
                .AddQueryParameter("to", to.ToString("yyyy-MM-dd"));

            return ExecuteAsync<HotelsResponse>(request, token);
        }
    }
}
