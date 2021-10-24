using LeadersApi.Models.Responses.Hotels;
using LeadersOfDigital.DataModels.Enums;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LeadersApi.Services.Hotels
{
    public class HotelsService : IHotelsService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMemoryCache _memoryCache;
        public HotelsService(IHttpClientFactory clientFactory, IMemoryCache memoryCache)
        {
            _clientFactory = clientFactory;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Hotel>> GetHotels(Cities city, DateTime from, DateTime to)
        {

            if (!_memoryCache.TryGetValue("Hotels", out IEnumerable<Hotel> cacheEntry) || (cacheEntry == null || !cacheEntry.Any()))
            {
                var request = new HttpRequestMessage(HttpMethod.Get,
              $"http://engine.hotellook.com/api/v2/cache.json?location={city}&currency=rub&checkIn={from:yyyy-MM-dd}&checkOut={to:yyyy-MM-dd}&limit=1000&lang=ru");

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);
                var responseStream = await response.Content.ReadAsStringAsync();
                cacheEntry = JsonConvert.DeserializeObject<IEnumerable<Hotel>>(responseStream);



                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromHours(1));

                // Save data in cache.
                _memoryCache.Set("Hotels", cacheEntry, cacheEntryOptions);
            }

            return cacheEntry;

        }

        public async Task<Hotel> GetSuggestedHotel(int maxBudget, int currentBudget, Cities city, DateTime from, DateTime to)
        {
            var allHotels = await GetHotels(city, from, to);
            var rand = new Random();

            // TODO: подбор по локации (по интересам пользователя)
            var hotels = allHotels.Where(x =>
            (x.priceAvg + currentBudget) < maxBudget * 0.7) //входим в бюджет до 70%, по статистике так и выходит 60-70%
                .ToList();

            return hotels.ElementAt(rand.Next(hotels.Count));
        }
    }
}
