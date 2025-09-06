using Domain.Models.Abstract;

namespace Domain.Models.Ride
{
    public class DeliverySlot: BaseModel
    {
        public Guid TripId { get; set; }
        public virtual Trip Trip { get; set; } = null!;

        public string CargoSlotTypeName { get; set; } = null!;
        public string MaxWeight { get; set; } = null!;
        public string MaxVolume { get; set; } = null!;

        public decimal ApproximatePrice { get; set; }

        public bool IsAvailable { get; set; } = true;
        public virtual DeliveryOrder? DeliveryOrder { get; set; }
    }
}
