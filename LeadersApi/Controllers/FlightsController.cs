using AutoMapper;
using LeadersApi.Models.Responses.Flights;
using LeadersApi.Models.Responses.Hotels;
using LeadersOfDigital.DataModels.Enums;
using LeadersOfDigital.DataModels.Responses.Flights;
using LeadersOfDigital.DataModels.Responses.Hotels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LeadersApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly ILogger<HotelsController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly string _token;


        public FlightsController(ILogger<HotelsController> logger, IHttpClientFactory clientFactory, IMapper mapper, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _token = configuration.GetValue<string>("AviasalesToken");
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(Iata origin, Iata destination,DateTime departure, DateTime @return)
        {
            try
            {

                    var request = new HttpRequestMessage(HttpMethod.Get,
                   $"https://api.travelpayouts.com/aviasales/v3/prices_for_dates?origin={origin}&destination={destination}&currency=rub&departure_at={departure:yyyy-MM-dd}&return_at={@return:yyyy-MM-dd}&sorting=price&direct=true&limit=100&token={_token}");

                    var client = _clientFactory.CreateClient();

                    var response = await client.SendAsync(request);
                    var responseStream = await response.Content.ReadAsStringAsync();
                    var flights = _mapper.Map<IEnumerable<FlightsResponse>>(JsonConvert.DeserializeObject<Flights>(responseStream).data);

                if (!flights.Any())
                {
                    return NoContent();
                }

                return Ok(flights);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка поиска билетов");

                return BadRequest("Ошибка поиска билетов");
            }
        }

    }
}
