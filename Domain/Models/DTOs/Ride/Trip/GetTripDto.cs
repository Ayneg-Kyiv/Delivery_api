using Domain.Models.DTOs.Identity;
using Domain.Models.DTOs.Ride.DeliveryOrder;
using Domain.Models.DTOs.Ride.DeliverySlot;
using Domain.Models.DTOs.Ride.Location;
using Domain.Models.Vehicles;

namespace Domain.Models.DTOs.Ride.Trip
{
    public class GetTripDto
    {
        public Guid Id { get; set; }

        public Guid DriverId { get; set; }
        public virtual GetApplicationUserForTripDto Driver { get; set; } = null!;

        public Guid StartLocationId { get; set; }
        public GetLocationDto StartLocation { get; set; } = null!;
        public Guid EndLocationId { get; set; }
        public GetLocationDto EndLocation { get; set; } = null!;

        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public Guid VehicleId { get; set; }
        public virtual Vehicle? Vehicle { get; set; }

        public List<GetDeliverySlotDto> DeliverySlots { get; set; } = [];
        public List<GetDeliveryOrderDto> DeliveryOrders { get; set; } = [];

        public bool IsStarted { get; set; } = false;
        public bool IsCompleted { get; set; } = false;

    }
}
