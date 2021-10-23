using AutoMapper;
using LeadersApi.Models.Responses.Hotels;
using LeadersApi.Services.Hotels;
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
        private readonly IMapper _mapper;
        private readonly IHotelsService _hotelsService;


        public HotelsController(ILogger<HotelsController> logger, IMapper mapper, IHotelsService hotelsService)
        {
            _logger = logger;
            _hotelsService = hotelsService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(Cities city,DateTime from, DateTime to, int pageSize = 10, int page = 1)
        {
            try
            {

                var hotels = _mapper.Map<IEnumerable<HotelsResponse>>(await _hotelsService.GetHotels(city, from, to));

                if (!hotels.Any())
                {
                    return NoContent();
                }

                return Ok(hotels.Skip((page - 1) * pageSize).Take(pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка поиска отелей");

                return BadRequest("Ошибка поиска отелей");
            }
        }

    }
}
