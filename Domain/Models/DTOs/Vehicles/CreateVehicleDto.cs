using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Vehicles
{
    public class CreateVehicleDto
    {
        [Required]
        public Guid OwnerId { get; set; }

        public string? Brand { get; set; }
        public string? Model { get; set; }
        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        public string NumberPlate { get; set; } = string.Empty;
        [Required]
        [SwaggerSchema(Description = "Color of vehicle")]
        public string Color { get; set; } = string.Empty;


        /// <summary>
        /// The image of the front of the vehicle.
        /// </summary>
        [Required]
        [DataType(DataType.Upload)]
        [SwaggerSchema(Format = "binary", Description = "The vehicle's front image")]
        public IFormFile ImageFront { get; set; } = null!;


        /// <summary>
        /// The image of the back of the vehicle.
        /// </summary>
        [Required]
        [DataType(DataType.Upload)]
        [SwaggerSchema(Format = "binary", Description = "The vehicle's back image")]
        public IFormFile ImageBack { get; set; } = null!;
    }
}
