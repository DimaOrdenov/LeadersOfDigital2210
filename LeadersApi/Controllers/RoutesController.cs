using System;
using System.Threading.Tasks;
using Dal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LeadersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoutesController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<RoutesController> _logger;

        public RoutesController(
            AppDbContext dbContext,
            ILogger<RoutesController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(DateTime startsAt, DateTime endsAt)
        {
            try
            {
                return Ok(await _dbContext.Routes
                    .Include(x => x.Origin)
                    .Include(x => x.Destination)
                    .ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't get routes");

                return BadRequest("Couldn't get routes");
            }
        }
    }
}
