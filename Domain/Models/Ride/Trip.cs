using Domain.Models.Abstract;
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

        public bool IsCompleted { get; set; } = false;
    }
}
