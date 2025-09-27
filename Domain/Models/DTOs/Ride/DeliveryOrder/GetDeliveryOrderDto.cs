using Domain.Models.DTOs.Identity;
using Domain.Models.DTOs.Ride.DeliverySlot;
using Domain.Models.DTOs.Ride.Location;

namespace Domain.Models.DTOs.Ride.DeliveryOrder
{
    public class GetDeliveryOrderDto
    {
        public Guid Id { get; set; }

        public Guid TripId { get; set; }
        public virtual object? Trip { get; set; }

        public Guid? SenderId { get; set; }
        public virtual GetApplicationUserForTripDto Sender { get; set; } = null!;

        public virtual GetApplicationUserForTripDto Driver { get; set; } = null!;

        public Guid DeliverySlotId { get; set; }
        public GetDeliverySlotDto? DeliverySlot { get; set; }

        public GetLocationDto StartLocation { get; set; } = null!;
        public GetLocationDto EndLocation { get; set; } = null!;

        public string SenderName { get; set; } = null!;
        public string SenderPhoneNumber { get; set; } = null!;
        public string? SenderEmail { get; set; }
        public string ReceiverName { get; set; } = null!;
        public string ReceiverPhoneNumber { get; set; } = null!;
        public string? Comment { get; set; }

        public bool IsAccepted { get; set; } = false;
        public bool IsDeclined { get; set; } = false;
        public bool IsPickedUp { get; set; } = false;
        public bool IsDelivered { get; set; } = false;
    }
}
