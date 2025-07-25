public class CreateShippingOrderDto
{
    public decimal EstimatedCost { get; set; }
    public float EstimatedDistance { get; set; }
    public DateOnly EstimatedShippingDate { get; set; }
    public TimeOnly EstimatedShippingTime { get; set; }
    public Guid CustomerId { get; set; }
}
