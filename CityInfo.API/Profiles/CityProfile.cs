using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;

namespace CityInfo.API.Profiles
{
    public class CityProfile: Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityWithoutPointOfInterestDto>(); // Map from City Object to CityWithoutPointOfInterestDto Object
            CreateMap<City, CityDto>();
            //CreateMap<City, CityDto>().ForMember(dest => dest.PointOfInterest, opt => opt.MapFrom(src => src.PointOfInterests)); // Ensure PointOfInterests is mapped to PointOfInterest
        }
    }
}
