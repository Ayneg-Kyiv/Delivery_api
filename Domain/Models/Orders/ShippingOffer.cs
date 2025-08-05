using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Orders
{
    public class ShippingOffer
    {
        [Key] public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        public Guid ShippingOrderId { get; set; }
        // Removed navigation property - using logical relationship via ID only
        // public ShippingOrder ShippingOrder { get; set; } = null!;

        // Логічний зв'язок з ApplicationUser через ID (без FK)
        public Guid CourierId { get; set; }

        public decimal OfferedPrice { get; set; }
        public DateOnly OfferedDate { get; set; }
        public TimeOnly OfferedTime { get; set; }
        public bool IsAccepted { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}