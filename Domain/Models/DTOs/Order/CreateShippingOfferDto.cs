namespace Application.DTOs.Orders
{
    public class CreateShippingOfferDto
    {
        public Guid ShippingOrderId { get; set; }
        public Guid CourierId { get; set; }

        public decimal OfferedPrice { get; set; }
        public DateOnly OfferedDate { get; set; }
        public TimeOnly OfferedTime { get; set; }

        public string Comment { get; set; } = string.Empty;
    }
}

