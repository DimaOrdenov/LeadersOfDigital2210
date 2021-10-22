using AutoMapper;
using LeadersApi.Models.Responses.Hotels;
using LeadersOfDigital.DataModels.Responses.Hotels;

namespace LeadersApi.Automapper
{
    public class HotelsProfile : Profile
    {
        public HotelsProfile()
        {
            CreateMap<Hotel, HotelsResponse>()
                .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.location.geo.lat))
                .ForMember(dest => dest.Lon, opt => opt.MapFrom(src => src.location.geo.lon))
                ;
        }
    }
}
