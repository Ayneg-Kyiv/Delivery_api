using Domain.Models.DTOs.Identity;
using Domain.Models.DTOs.Ride.DeliveryOffer;
using Domain.Models.DTOs.Ride.Location;

namespace Domain.Models.DTOs.Ride.DeliveryRequest
{
    public class GetDeliveryRequestDto
    {
        public Guid Id { get; set; }

        public Guid StartLocationId { get; set; }
        public GetLocationDto StartLocation { get; set; } = null!;

        public Guid EndLocationId { get; set; }
        public GetLocationDto EndLocation { get; set; } = null!;

        public Guid SenderId { get; set; }
        public virtual GetApplicationUserForTripDto Sender { get; set; } = null!;

        public string SenderName { get; set; } = null!;
        public string SenderPhoneNumber { get; set; } = null!;
        public string? SenderEmail { get; set; }

        public string ReceiverName { get; set; } = null!;
        public string ReceiverPhoneNumber { get; set; } = null!;

        public string ObjectName { get; set; } = null!;
        public string CargoSlotType { get; set; } = null!;
        public double ObjectWeight { get; set; }

        public string? ObjectDescription { get; set; }
        public string? Comment { get; set; }

        public decimal? EstimatedPrice { get; set; }

        public Guid? DeliveryOfferID { get; set; }
        
        public virtual ICollection<GetDeliveryOfferDto> Offers { get; set; } = [];

        public bool IsAccepted { get; set; } = false;
        public bool IsPickedUp { get; set; } = false;
        public bool IsDelivered { get; set; } = false;
    }
}
