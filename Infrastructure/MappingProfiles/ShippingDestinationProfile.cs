using AutoMapper;
using Application.DTOs.Orders;
using Domain.Models.Orders;

namespace Application.MappingProfiles
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
