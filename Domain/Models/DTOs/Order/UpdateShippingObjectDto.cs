namespace Domain.Models.DTOs.Order
{
    public class UpdateShippingObjectDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public float? Weight { get; set; }
        public float? Width { get; set; }
        public float? Height { get; set; }
        public float? Length { get; set; }

        public string? ImagePath { get; set; }
    }
}
