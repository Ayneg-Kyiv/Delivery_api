using AutoMapper;
using Domain.Models.DTOs.Ride.DeliveryRequest;
using Domain.Models.Ride;

namespace Infrastructure.MappingProfiles
{
    public class DeliveryRequestProfile: Profile
    {
        public DeliveryRequestProfile()
        {
            CreateMap<DeliveryRequest, GetDeliveryRequestDto>();
            CreateMap<CreateDeliveryRequestDto, DeliveryRequest>();
        }
    }
}
