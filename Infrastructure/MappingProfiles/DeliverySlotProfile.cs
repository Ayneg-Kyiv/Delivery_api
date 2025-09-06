using AutoMapper;
using Domain.Models.DTOs.Ride.DeliverySlot;
using Domain.Models.Ride;

namespace Infrastructure.MappingProfiles
{
    public class DeliverySlotProfile : Profile
    {
        public DeliverySlotProfile()
        {
            CreateMap<DeliverySlot, GetDeliverySlotDto>();
            CreateMap<CreateDeliverySlotDto, DeliverySlot>();
        }
    }
}
