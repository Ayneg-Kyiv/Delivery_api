using Domain.Models.DTOs.Identity;
using Domain.Models.DTOs.Order;
using Domain.Models.DTOs.Ride.DeliveryOrder;
using Domain.Models.DTOs.Ride.DeliveryRequest;

namespace Domain.Models.DTOs.Reviews
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; } = string.Empty;

        public Guid? ShippingOrderId { get; set; }
        public virtual ShippingOrderDto ShippingOrder { get; set; } = null!;

        public Guid? DeliveryOrderId { get; set; }
        public virtual GetDeliveryOrderDto DeliveryOrder { get; set; } = null!;

        public Guid? DeliveryRequestId { get; set; }
        public GetDeliveryRequestDto? DeliveryRequest { get; set; } = null!;

        public Guid UserId { get; set; }
        public Guid ReviewerId { get; set; }

        public GetApplicationUserForTripDto? Reviewer { get; set; } 
    }
}