using AutoMapper;
using Application.DTOs.Orders;
using Domain.Models.Orders;

namespace Application.MappingProfiles
{
    public class ShippingOrderProfile : Profile
    {
        public ShippingOrderProfile()
        {
            // ShippingOrder → ShippingOrderDto (для перегляду)
            CreateMap<ShippingOrder, ShippingOrderDto>();

            // CreateShippingOrderDto → ShippingOrder (для створення)
            CreateMap<CreateShippingOrderDto, ShippingOrder>();

            // UpdateShippingOrderDto → ShippingOrder (для оновлення)
            CreateMap<UpdateShippingOrderDto, ShippingOrder>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
