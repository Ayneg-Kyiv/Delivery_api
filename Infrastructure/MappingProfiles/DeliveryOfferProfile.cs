using AutoMapper;
using Domain.Models.DTOs.Ride.DeliveryOffer;
using Domain.Models.Ride;

namespace Infrastructure.MappingProfiles
{
    public class DeliveryOfferProfile : Profile
    {
        public DeliveryOfferProfile()
        {
            CreateMap<DeliveryOffer, GetDeliveryOfferDto>();
            CreateMap<CreateDeliveryOfferDto, DeliveryOffer>();
        }
    }
}
