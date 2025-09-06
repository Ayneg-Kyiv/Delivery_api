using Domain.Models.DTOs.Ride.DeliverySlot;
using Domain.Models.DTOs.Ride.Location;

namespace Domain.Models.DTOs.Ride.Trip
{
    public class CreateTripDto
    {
        public Guid DriverId { get; set; }
        public CreateLocationDto StartLocation { get; set; } = null!;
        public CreateLocationDto EndLocation { get; set; } = null!;

        public List<CreateDeliverySlotDto> Slots { get; set; } = [];
    }
}
