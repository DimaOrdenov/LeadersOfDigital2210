using LeadersApi.Models.Responses.Flights;
using LeadersOfDigital.DataModels.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeadersApi.Services.Flights
{
    public interface IFlightsService
    {
        Task<IEnumerable<Flight>> GetFlights(Iata origin, Iata destination, DateTime departure, DateTime @return);
    }
}