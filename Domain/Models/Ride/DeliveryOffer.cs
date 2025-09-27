using Domain.Models.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Ride
{
    public class DeliveryOffer: BaseModel
    {
        public Guid DeliveryRequestId { get; set; }

        public virtual DeliveryRequest DeliveryRequest { get; set; } = null!;
        public decimal Price { get; set; }

        public Guid DriverId { get; set; }

        public DateTime EstimatedCollectionTime { get; set; }
        public DateTime EstimatedDeliveryTime { get; set; }

        public bool IsAccepted { get; set; } = false;
        public bool IsDeclined { get; set; } = false;
    }
}
