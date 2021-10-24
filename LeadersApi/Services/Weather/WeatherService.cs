using LeadersOfDigital.DataModels.Enums;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LeadersApi.Services.Weather
{
    public class WeatherService : IWeatherService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _token;

        public WeatherService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _token = configuration.GetValue<string>("WeatherApiToken");
        }

        public async Task<Models.Responses.Weather.Weather> GetWeatherForecast(Cities city)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
              $"https://weatherapi-com.p.rapidapi.com/forecast.json?q={city}&days=3");
            request.Headers.Add("x-rapidapi-host", "weatherapi-com.p.rapidapi.com");
            request.Headers.Add("x-rapidapi-key",_token);
            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);
            var responseStream = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Models.Responses.Weather.Weather>(responseStream);
        }
    }
}