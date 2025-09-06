using Domain.Models.DTOs.Ride.Location;

namespace Domain.Models.DTOs.Ride.DeliveryOrder
{
    public class GetDeliveryOrderDto
    {
        public Guid Id { get; set; }

        public Guid TripId { get; set; }
        public Guid DeliverySlotId { get; set; }

        public GetLocationDto StartLocation { get; set; } = null!;
        public GetLocationDto EndLocation { get; set; } = null!;

        public string SenderName { get; set; } = null!;
        public string SenderPhoneNumber { get; set; } = null!;
        public string? SenderEmail { get; set; }
        public string ReceiverName { get; set; } = null!;
        public string ReceiverPhoneNumber { get; set; } = null!;
        public string? Comment { get; set; }
    }
}
