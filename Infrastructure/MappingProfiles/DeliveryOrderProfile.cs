using AutoMapper;
using Domain.Models.DTOs.Ride.DeliveryOrder;
using Domain.Models.Ride;

namespace Infrastructure.MappingProfiles
{
    public class DeliveryOrderProfile: Profile
    {
        public DeliveryOrderProfile()
        {
            CreateMap<DeliveryOrder, GetDeliveryOrderDto>();
            CreateMap<CreateDeliveryOrderDto, DeliveryOrder>();
        }
    }
}
