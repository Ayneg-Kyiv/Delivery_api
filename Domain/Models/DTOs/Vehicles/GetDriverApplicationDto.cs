using Application.DTOs.Vehicles;
using Domain.Models.Vehicles;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DTOs.Vehicles
{
    public class GetDriverApplicationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;

        [MaxLength(512)]
        [Description("User's profile image Path")]
        public string? ImagePath { get; set; }

        [MaxLength(512)]
        [Description("Driver license image to verify user, not seen by other")]
        public string? DriverLicenseImagePath { get; set; }

        public string? Email { get; set; }
    }
}
