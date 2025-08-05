namespace Domain.Models.DTOs.Order
{
    public class ShippingObjectDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public float Weight { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Length { get; set; }

        public Guid ShippingOrderId { get; set; }
        public string? ImagePath { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
