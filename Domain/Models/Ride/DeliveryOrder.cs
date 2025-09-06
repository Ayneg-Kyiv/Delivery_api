using Domain.Models.Abstract;
using Domain.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Ride
{
    public class DeliveryOrder : BaseModel
    {
        [Required]
        public Guid TripId { get; set; }
        public virtual Trip Trip { get; set; } = null!;

        [Required]
        public Guid DeliverySlotId { get; set; }
        public virtual DeliverySlot DeliverySlot { get; set; } = null!;

        [Required]
        public Guid StartLocationId { get; set; }
        public virtual Location StartLocation { get; set; } = null!;
        [Required]
        public Guid EndLocationId { get; set; }
        public Location EndLocation { get; set; } = null!;


        [Required]
        public Guid SenderId { get; set; }

        [Required]
        public string SenderName { get; set; } = null!;
        [Required]
        public string SenderPhoneNumber { get; set; } = null!;
        public string? SenderEmail { get; set; }

        [Required]
        public string ReceiverName { get; set; } = null!;
        [Required]
        public string ReceiverPhoneNumber { get; set; } = null!;

        public string? Comment { get; set; }

        public bool IsPickedUp { get; set; } = false;
        public bool IsDelivered { get; set; } = false;
    }
}
