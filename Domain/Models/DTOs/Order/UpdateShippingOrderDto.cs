namespace Application.DTOs.Orders
{
    public class UpdateShippingOrderDto
    {
        public decimal? EstimatedCost { get; set; }
        public float? EstimatedDistance { get; set; }

        public DateOnly? EstimatedShippingDate { get; set; }
        public TimeOnly? EstimatedShippingTime { get; set; }
    }
}
