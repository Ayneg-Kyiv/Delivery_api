using AutoMapper;
using Domain.Models.DTOs.Order;
using Domain.Models.Orders;

namespace Infrastructure.MappingProfiles
{
    public class ShippingStartingPointProfile : Profile
    {
        public ShippingStartingPointProfile()
        {
            // Domain → DTO
            CreateMap<ShippingStartingPoint, ShippingStartingPointDto>()
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District ?? string.Empty))
                .ForMember(dest => dest.HouseNumber, opt => opt.MapFrom(src => src.HouseNumber ?? string.Empty))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber ?? string.Empty))
                .ForMember(dest => dest.AdditionalInfo, opt => opt.MapFrom(src => src.AdditionalInfo ?? string.Empty));

            // Create DTO → Domain
            CreateMap<CreateShippingStartingPointDto, ShippingStartingPoint>();

            // Update DTO → Domain
            CreateMap<UpdateShippingStartingPointDto, ShippingStartingPoint>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
