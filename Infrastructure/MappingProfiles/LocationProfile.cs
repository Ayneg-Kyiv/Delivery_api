using AutoMapper;
using Domain.Models.DTOs.Ride.Location;
using Domain.Models.Ride;

namespace Infrastructure.MappingProfiles
{
    public class LocationProfile: Profile
    {
        public LocationProfile()
        {
            CreateMap<CreateLocationDto, Location>();
            CreateMap<Location, GetLocationDto>();
        }
    }
}
