using Domain.Models.DTOs.Ride.Location;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DTOs.Ride.DeliveryRequest
{
    public class CreateDeliveryRequestDto
    {
        // Locations can be created on the fly if they don't exist
        public Guid? StartLocationId { get; set; }
        public CreateLocationDto? StartLocation { get; set; }

        // Locations can be created on the fly if they don't exist
        public Guid? EndLocationId { get; set; }
        public CreateLocationDto? EndLocation { get; set; }

        public Guid SenderId { get; set; }

        [Required]
        public string SenderName { get; set; } = null!;
        [Required]
        public string SenderPhoneNumber { get; set; } = null!;
        public string? SenderEmail { get; set; }

        [Required]
        public string ObjectName { get; set; } = null!;
        [Required]
        public string CargoSlotType { get; set; } = null!;
        [Required]
        public double ObjectWeight { get; set; }

        public string? ObjectDescription { get; set; }

        public decimal? EstimatedPrice { get; set; }


        [Required]
        public string ReceiverName { get; set; } = null!;
        [Required]
        public string ReceiverPhoneNumber { get; set; } = null!;

        public string? Comment { get; set; }
    }
}
