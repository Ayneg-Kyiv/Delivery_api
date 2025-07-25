namespace Application.DTOs.Orders
{
    public class CreateShippingOrderDto
    {
        public Guid CustomerId { get; set; }

        public decimal EstimatedCost { get; set; }
        public float EstimatedDistance { get; set; }

        public DateOnly EstimatedShippingDate { get; set; }
        public TimeOnly EstimatedShippingTime { get; set; }
    }
}
