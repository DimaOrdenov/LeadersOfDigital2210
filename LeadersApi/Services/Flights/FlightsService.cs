using LeadersApi.Models.Responses.Flights;
using LeadersOfDigital.DataModels.Enums;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using models =  LeadersApi.Models.Responses.Flights;

namespace LeadersApi.Services.Flights
{
    public class FlightsService : IFlightsService
    {

        private readonly IHttpClientFactory _clientFactory;
        private readonly string _token;


        public FlightsService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _token = configuration.GetValue<string>("AviasalesToken");
        }

        public async Task<IEnumerable<Flight>> GetFlights(Iata origin, Iata destination, DateTime departure, DateTime @return)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://api.travelpayouts.com/aviasales/v3/prices_for_dates?origin={origin}&destination={destination}&currency=rub&departure_at={departure:yyyy-MM-dd}" + ((@return!=DateTime.MinValue) ? $"&return_at={@return:yyyy-MM-dd}":null)+ $"&sorting=price&direct=true&limit=100&token={_token}");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);
            var responseStream = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<models.Flights>(responseStream).data;
        }
    }
}
