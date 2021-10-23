using Dal;
using Dal.Entities;
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

        public WeatherController(AppDbContext dbContext, ILogger<WeatherController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
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
    }
}