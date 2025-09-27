using Domain.Models.Abstract;
using Domain.Models.Orders;
using Domain.Models.Ride;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Reviews
{
    public class Review: BaseModel
    {
        // Логічний зв'язок з ApplicationUser через ID (без FK)
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ReviewerId { get; set; }

        public Guid? ShippingOrderId { get; set; }
        public virtual ShippingOrder ShippingOrder { get; set; } = null!;

        public Guid? DeliveryOrderId { get; set; }
        public virtual DeliveryOrder DeliveryOrder { get; set; } = null!;

        public Guid? DeliveryRequestId { get; set; }
        public DeliveryRequest? DeliveryRequest { get; set; } = null!;

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [MaxLength(512)]
        public string Text { get; set; } = string.Empty;
    }
}
