using Domain.Models.DTOs.Identity;
using Domain.Models.DTOs.Ride.DeliveryOrder;
using Domain.Models.DTOs.Ride.DeliverySlot;
using Domain.Models.DTOs.Ride.Location;

namespace Domain.Models.DTOs.Ride.Trip
{
    public class GetTripDto
    {
        public Guid Id { get; set; }

        public Guid DriverId { get; set; }
        public GetApplicationUserForTripDto Driver { get; set; } = null!;

        public GetLocationDto StartLocation { get; set; } = null!;
        public GetLocationDto EndLocation { get; set; } = null!;

        public List<GetDeliverySlotDto> DeliverySlots { get; set; } = [];
        public List<GetDeliveryOrderDto> DeliveryOrders { get; set; } = [];

    }
}
