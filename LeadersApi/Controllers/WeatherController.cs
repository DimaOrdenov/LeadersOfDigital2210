using Dal;
using Dal.Entities;
using LeadersApi.Services.Weather;
using LeadersOfDigital.DataModels.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LeadersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherService _weatherService;


        public WeatherController(AppDbContext dbContext, ILogger<WeatherController> logger, IWeatherService weatherService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                return Ok(await _dbContext.Weathers.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't get weather");

                return BadRequest("Couldn't get weather");
            }
        }

        [HttpGet("city")]
        public async Task<IActionResult> GetAsync(Cities city)
        {
            try
            {
                return Ok(await _weatherService.GetWeatherForecast(city));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't get weather");

                return BadRequest("Couldn't get weather");
            }
        }
    }
}