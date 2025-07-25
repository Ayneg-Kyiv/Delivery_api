using AutoMapper;
using Application.DTOs.Orders;
using Domain.Models.Orders;

namespace Application.MappingProfiles
{
    public class ShippingObjectProfile : Profile
    {
        public ShippingObjectProfile()
        {
            // ShippingObject → ShippingObjectDto
            CreateMap<ShippingObject, ShippingObjectDto>();

            // CreateShippingObjectDto → ShippingObject
            CreateMap<CreateShippingObjectDto, ShippingObject>();

            // UpdateShippingObjectDto → ShippingObject
            CreateMap<UpdateShippingObjectDto, ShippingObject>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
