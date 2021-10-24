using System;
using System.Threading;
using System.Threading.Tasks;
using LeadersOfDigital.DataModels.Responses.Hotels;

namespace Business
{
    public interface IHotelsService
    {
        Task<HotelsResponse> GetSuggestedHotelAsync(int maxBudget, int currentBudget, int city, DateTime from, DateTime to, CancellationToken token);
    }
}
