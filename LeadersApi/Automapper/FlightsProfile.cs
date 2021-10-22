using AutoMapper;
using LeadersApi.Models.Responses.Flights;
using LeadersOfDigital.DataModels.Responses.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadersApi.Automapper
{
    public class FlightsProfile : Profile
    {
        public FlightsProfile()
        {
            CreateMap<Flight, FlightsResponse>()
                .ForMember(dest => dest.Link, opt => opt.MapFrom(src => "https://www.aviasales.ru" + src.link));
        }
    }
}
