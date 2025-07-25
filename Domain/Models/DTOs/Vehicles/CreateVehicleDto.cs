namespace Application.DTOs.Vehicles
{
    public class CreateVehicleDto
    {
        public Guid OwnerId { get; set; }

        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string Type { get; set; } = string.Empty;

        public string NumberPlate { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string? ImagePath { get; set; }
    }
}
