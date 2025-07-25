using Domain.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Orders
{
    public class ShippingOffer
    {
        [Key] public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        public Guid ShippingOrderId { get; set; }
        public ShippingOrder ShippingOrder { get; set; } = null!;

        public Guid CourierId { get; set; }
        public ApplicationUser Courier { get; set; } = null!;

        public decimal OfferedPrice { get; set; }
        public DateOnly OfferedDate { get; set; }
        public TimeOnly OfferedTime { get; set; }
        public bool IsAccepted { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}