using LeadersApi.Services.Trips;
using LeadersOfDigital.DataModels.Requests.Trips;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LeadersApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripsService _tripsService;
        private readonly ILogger<TripController> _logger;

        public TripController(ITripsService tripsService, ILogger<TripController> logger)
        {
            _tripsService = tripsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var trips = await _tripsService.GetTrips();

                if (!trips.Any())
                {
                    return NoContent();
                }

                return Ok(trips);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения маршрутов");

                return BadRequest("Ошибка получения маршрутов");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                var trip = await _tripsService.GetTrip(id);

                if (trip == null)
                {
                    return NoContent();
                }

                return Ok(trip);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения маршрута");

                return BadRequest("Ошибка получения маршрута");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateTripRequest request)
        {
            try
            {
                var trip = await _tripsService.CreateTrip(request);

                return CreatedAtAction(nameof(CreateAsync), trip);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения маршрута");

                return BadRequest("Ошибка получения маршрута");
            }
        }


        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UpdateTripRequest request)
        {
            try
            {
                var trip = await _tripsService.UpdateTrip(request);

                return Ok(trip);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения маршрута");

                return BadRequest("Ошибка получения маршрута");
            }
        }
    }
}