using AutoMapper;
using Domain.Models.DTOs.Order;
using Domain.Models.Orders;

namespace Application.MappingProfiles
{
    public class ShippingOfferProfile : Profile
    {
        public ShippingOfferProfile()
        {
            // ShippingOffer → ShippingOfferDto (для перегляду)
            CreateMap<ShippingOffer, ShippingOfferDto>();

            // CreateShippingOfferDto → ShippingOffer (для створення)
            CreateMap<CreateShippingOfferDto, ShippingOffer>();

            // UpdateShippingOfferDto → ShippingOffer (для часткового оновлення)
            CreateMap<UpdateShippingOfferDto, ShippingOffer>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
