using AutoMapper;
using Domain.Models.DTOs.Order;
using Domain.Models.Orders;

namespace Infrastructure.MappingProfiles
{
    public class ShippingDestinationProfile : Profile
    {
        public ShippingDestinationProfile()
        {
            // Domain → DTO
            CreateMap<ShippingDestination, ShippingDestinationDto>();

            // Create DTO → Domain
            CreateMap<CreateShippingDestinationDto, ShippingDestination>();

            // Update DTO → Domain
            CreateMap<UpdateShippingDestinationDto, ShippingDestination>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
