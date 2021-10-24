using LeadersOfDigital.DataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using model = LeadersApi.Models.Responses.Weather;

namespace LeadersApi.Services.Weather
{
    public interface IWeatherService
    {
        Task<model.Weather> GetWeatherForecast(Cities city);
    }
}
