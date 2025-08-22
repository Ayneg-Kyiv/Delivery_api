namespace Domain.Models.DTOs.Order
{
    public class ShippingOfferDto
    {
        public Guid Id { get; set; }

        public Guid ShippingOrderId { get; set; }
        public Guid CourierId { get; set; }

        public decimal OfferedPrice { get; set; }
        public DateOnly OfferedDate { get; set; }
        public TimeOnly OfferedTime { get; set; }

        public bool IsAccepted { get; set; }
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
