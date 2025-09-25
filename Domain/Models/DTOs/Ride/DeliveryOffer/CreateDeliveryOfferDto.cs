namespace Domain.Models.DTOs.Ride.DeliveryOffer
{
    public class CreateDeliveryOfferDto
    {
        public Guid DeliveryRequestId { get; set; }
        public decimal Price { get; set; }

        public Guid DriverId { get; set; }
        public DateTime EstimatedCollectionTime { get; set; }
        public DateTime EstimatedDeliveryTime { get; set; }
    }
}
