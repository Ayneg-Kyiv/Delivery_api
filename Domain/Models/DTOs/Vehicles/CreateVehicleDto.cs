using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

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


        [DataType(DataType.Upload)]
        public IFormFile ImageFront { get; set; } = null!;

        [DataType(DataType.Upload)]
        public IFormFile ImageBack { get; set; } = null!;
    }
}
