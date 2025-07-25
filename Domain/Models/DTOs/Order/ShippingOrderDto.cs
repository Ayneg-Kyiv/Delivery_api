namespace Application.DTOs.Orders
{
    public class ShippingOrderDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        public Guid CustomerId { get; set; }

        public decimal EstimatedCost { get; set; }
        public float EstimatedDistance { get; set; }

        public DateOnly EstimatedShippingDate { get; set; }
        public TimeOnly EstimatedShippingTime { get; set; }
    }
}
