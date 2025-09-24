using Domain.Models.Abstract;

namespace Domain.Models.Vehicles
{
    public class DriverApplication : BaseModel
    {
        public Guid UserId { get; set; }

        public Guid VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; } = null!;
    }
}
