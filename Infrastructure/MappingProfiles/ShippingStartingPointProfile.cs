using AutoMapper;
using Application.DTOs.Orders;
using Domain.Models.Orders;

namespace Application.MappingProfiles
{
    public class ShippingStartingPointProfile : Profile
    {
        public ShippingStartingPointProfile()
        {
            // Domain → DTO
            CreateMap<ShippingStartingPoint, ShippingStartingPointDto>();

            // Create DTO → Domain
            CreateMap<CreateShippingStartingPointDto, ShippingStartingPoint>();

            // Update DTO → Domain
            CreateMap<UpdateShippingStartingPointDto, ShippingStartingPoint>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
