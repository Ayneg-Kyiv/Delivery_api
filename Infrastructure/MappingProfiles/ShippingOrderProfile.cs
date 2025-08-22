using AutoMapper;
using Domain.Models.DTOs.Order;
using Domain.Models.Orders;

namespace Infrastructure.MappingProfiles
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
