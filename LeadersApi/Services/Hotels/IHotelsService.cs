using LeadersApi.Models.Responses.Hotels;
using LeadersOfDigital.DataModels.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeadersApi.Services.Hotels
{
    public interface IHotelsService
    {
        Task<IEnumerable<Hotel>> GetHotels(Cities city, DateTime from, DateTime to);
    }
}