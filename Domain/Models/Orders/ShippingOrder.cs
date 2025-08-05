using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Orders
{
    public class ShippingOrder
    {
        [Key] public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        // Логічний зв'язок з ApplicationUser через ID (без FK)
        public Guid CustomerId { get; set; }

        public decimal EstimatedCost { get; set; }
        public float EstimatedDistance { get; set; }
        public DateOnly EstimatedShippingDate { get; set; }
        public TimeOnly EstimatedShippingTime { get; set; }

        public ICollection<ShippingOffer>? Offers { get; set; }
        public ICollection<ShippingObject>? Objects { get; set; }
        public ICollection<ShippingDestination>? ShippingDestinations { get; set; }
    }
}