using Domain.Models.Abstract;
using Domain.Models.Vehicles;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Ride
{
    public class Trip : BaseModel
    {
        [Required]
        public Guid DriverId { get; set; }

        public Guid StartLocationId { get; set; }
        public virtual Location StartLocation { get; set; } = null!;
        public Guid EndLocationId { get; set; }
        public virtual Location EndLocation { get; set; } = null!;

        public virtual List<DeliverySlot> Slots { get; set; } = [];
        public virtual List<DeliveryOrder> Orders { get; set; } = [];


        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public Guid VehicleId { get; set; }
        public virtual Vehicle? Vehicle { get; set; }

        public bool IsStarted { get; set; } = false;
        public bool IsCompleted { get; set; } = false;
    }
}
