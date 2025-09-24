using Application.DTOs.Vehicles;

namespace Domain.Models.DTOs.Vehicles
{
    public class GetDriverApplicationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public Guid VehicleId { get; set; }
        public VehicleDto Vehicle { get; set; } = null!;
    }
}
