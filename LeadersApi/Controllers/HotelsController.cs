using AutoMapper;
using LeadersApi.Models.Responses.Hotels;
using LeadersOfDigital.DataModels.Enums;
using LeadersOfDigital.DataModels.Responses.Hotels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
    public class HotelsController : ControllerBase
    {
        private readonly ILogger<HotelsController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;


        public HotelsController(ILogger<HotelsController> logger, IHttpClientFactory clientFactory, IMapper mapper, IMemoryCache memoryCache)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(Cities city,DateTime from, DateTime to, int pageSize = 10, int page = 1)
        {
            try
            {

                if (!_memoryCache.TryGetValue("Hotels", out IEnumerable<HotelsResponse> cacheEntry))
                {
                    var request = new HttpRequestMessage(HttpMethod.Get,
                   $"http://engine.hotellook.com/api/v2/cache.json?location={city}&currency=rub&checkIn={from:yyyy-MM-dd}&checkOut={to:yyyy-MM-dd}&limit=1000&lang=ru");

                    var client = _clientFactory.CreateClient();

                    var response = await client.SendAsync(request);
                    var responseStream = await response.Content.ReadAsStringAsync();
                    var hotels = JsonConvert.DeserializeObject<IEnumerable<Hotel>>(responseStream);
                    cacheEntry = _mapper.Map<IEnumerable<HotelsResponse>>(hotels);

                    // Set cache options.
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromHours(1));

                    // Save data in cache.
                    _memoryCache.Set("Hotels", cacheEntry, cacheEntryOptions);
                }


                if (!cacheEntry.Any())
                {
                    return NoContent();
                }

                return Ok(cacheEntry.Skip((page - 1) * pageSize).Take(pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка поиска отелей");

                return BadRequest("Ошибка поиска отелей");
            }
        }

    }
}
