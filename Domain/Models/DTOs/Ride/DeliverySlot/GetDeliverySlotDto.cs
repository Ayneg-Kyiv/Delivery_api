namespace Domain.Models.DTOs.Ride.DeliverySlot
{
    public class GetDeliverySlotDto
    {
        public Guid Id { get; set; }

        public string CargoSlotTypeName { get; set; } = null!;
        public string MaxWeight { get; set; } = null!;
        public string MaxVolume { get; set; } = null!;

        public decimal ApproximatePrice { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}
