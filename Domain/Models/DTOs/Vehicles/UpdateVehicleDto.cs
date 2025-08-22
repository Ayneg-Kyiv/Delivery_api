namespace Application.DTOs.Vehicles
{
    public class UpdateVehicleDto
    {
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Type { get; set; }

        public string? NumberPlate { get; set; }
        public string? Color { get; set; }
        public string? ImagePath { get; set; }
    }
}
