using Domain.Models.DTOs.Identity;
using Domain.Models.DTOs.Ride.DeliveryRequest;

namespace Domain.Models.DTOs.Ride.DeliveryOffer
{
    public class GetDeliveryOfferDto
    {
        public Guid Id { get; set; }

        public Guid DeliveryRequestId { get; set; }
        public GetDeliveryRequestDto DeliveryRequest { get; set; } = null!;
        public decimal Price { get; set; }

        public Guid DriverId { get; set; }
        public GetApplicationUserForTripDto Driver { get; set; } = null!;

        public DateTime EstimatedCollectionTime { get; set; }
        public DateTime EstimatedDeliveryTime { get; set; }

        public bool IsAccepted { get; set; } = false;
        public bool IsDeclined { get; set; } = false;
    }
}
