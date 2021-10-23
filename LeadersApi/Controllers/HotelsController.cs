using AutoMapper;
using LeadersApi.Services.Hotels;
using LeadersOfDigital.DataModels.Enums;
using LeadersOfDigital.DataModels.Responses.Hotels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> GetAsync(Cities city, DateTime from, DateTime to, int pageSize = 10, int page = 1)
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

        [HttpGet("suggested")]
        public async Task<IActionResult> GetSuggestedAsync(int maxBudget, int currentBudget, Cities city, DateTime from, DateTime to)
        {
            try
            {
                var hotel = _mapper.Map<HotelsResponse>(await _hotelsService.GetSuggestedHotel(maxBudget, currentBudget, city, from, to));

                if (hotel == null)
                {
                    return NoContent();
                }

                return Ok(hotel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось подобрать отель");

                return BadRequest("Не удалось подобрать отель");
            }
        }

    }
}