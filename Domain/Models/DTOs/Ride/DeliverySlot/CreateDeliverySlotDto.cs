using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DTOs.Ride.DeliverySlot
{
    public class CreateDeliverySlotDto
    {
        public decimal ApproximatePrice { get; set; }
        
        [Required]
        public string CargoSlotTypeName { get; set; } = null!;
    }
}
