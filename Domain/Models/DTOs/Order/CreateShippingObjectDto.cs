namespace Application.DTOs.Orders
{
    public class CreateShippingObjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public double Weight { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }

        public Guid ShippingOrderId { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }
}
