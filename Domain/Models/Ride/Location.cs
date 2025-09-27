using Domain.Models.Abstract;

namespace Domain.Models.Ride
{
    public class Location: BaseModel
    {
        public string Country { get; set; } = null!;
        public string? State { get; set; }
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? HouseNumber { get; set; }

        public DateTime DateTime { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public virtual ICollection<Trip> TripsStart { get; set; } = [];
        public virtual ICollection<Trip> TripsEnd { get; set; } = [];

        public virtual ICollection<DeliveryOrder> DeliveryOrderStartLocations { get; set; } = [];
        public virtual ICollection<DeliveryOrder> DeliveryOrderEndLocations { get; set; } = [];

        public virtual ICollection<DeliveryRequest> DeliveryRequestsStartLocations { get; set; } = [];
        public virtual ICollection<DeliveryRequest> DeliveryRequestsEndLocations { get; set; } = [];
    }
}
