using AutoMapper;
using Domain.Models.DTOs.Ride.Trip;
using Domain.Models.Ride;

namespace Infrastructure.MappingProfiles
{
    public class TripProfile: Profile
    {
        public TripProfile()
        {
            CreateMap<Trip, GetTripDto>()
                .ForMember(m=>m.StartLocation, opt => opt.MapFrom(src => src.StartLocation))
                .ForMember(m=>m.EndLocation, opt => opt.MapFrom(src => src.EndLocation))
                .ForMember(m=>m.DeliverySlots, opt => opt.MapFrom(src => src.Slots))
                .ForMember(m=>m.DeliveryOrders, opt => opt.MapFrom(src => src.Orders));
            CreateMap<CreateTripDto, Trip>();
        }
    }
}
