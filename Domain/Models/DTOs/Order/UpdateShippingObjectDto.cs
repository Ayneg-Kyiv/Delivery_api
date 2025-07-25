namespace Application.DTOs.Orders
{
    public class UpdateShippingObjectDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public double? Weight { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public double? Length { get; set; }

        public string? ImagePath { get; set; }
    }
}
