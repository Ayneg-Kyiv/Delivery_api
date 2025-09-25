using Domain.Models.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Ride
{
    public class DeliveryRequest : BaseModel
    {
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

        public string ObjectName { get; set; } = null!;
        public string CargoSlotType { get; set; } = null!;
        public double ObjectWeight { get; set; }

        public string? ObjectDescription { get; set; }

        public string? Comment { get; set; }

        public decimal? EstimatedPrice { get; set; }

        public Guid? DeliveryOfferID { get; set; }
        
        public virtual ICollection<DeliveryOffer> Offers { get; set; } = [];

        public bool IsAccepted { get; set; } = false;
        public bool IsPickedUp { get; set; } = false;
        public bool IsDelivered { get; set; } = false;
    }
}
