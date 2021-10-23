using System.Threading;
using System.Threading.Tasks;
using Business.Definitions.Requests;
using Business.Definitions.Responses;

namespace Business
{
    public interface IGoogleMapsApiService
    {
        Task<GoogleDirectionsResponse> GetDirectionsAsync(GoogleApiDirectionsRequest data, CancellationToken token);

        Task<GooglePlacesResponse> GetPlacesAsync(GoogleApiPlacesRequest data, CancellationToken token);
    }
}
