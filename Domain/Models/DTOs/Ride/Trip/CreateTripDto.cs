using Domain.Models.DTOs.Ride.DeliverySlot;
using Domain.Models.DTOs.Ride.Location;
using Domain.Models.Vehicles;

namespace Domain.Models.DTOs.Ride.Trip
{
    public class CreateTripDto
    {
        public Guid? DriverId { get; set; }
        public CreateLocationDto StartLocation { get; set; } = null!;
        public CreateLocationDto EndLocation { get; set; } = null!;

        public List<CreateDeliverySlotDto> Slots { get; set; } = [];

        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public Guid VehicleId { get; set; }
    }
}
