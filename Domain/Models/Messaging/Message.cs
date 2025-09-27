using Domain.Models.Abstract;
using Domain.Models.Orders;
using Domain.Models.Ride;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Messaging
{
    public class Message: BaseModel
    {
        // Логічні зв'язки з ApplicationUser через ID (без FK)
        [Required]
        public Guid SenderId { get; set; }

        [Required]
        public Guid ReceiverId { get; set; }

        public Guid? DeliveryOrderId { get; set; }
        public DeliveryOrder? DeliveryOrder { get; set; }

        public Guid? DeliveryOfferId { get; set; }
        public DeliveryOffer? DeliveryOffer { get; set; }

        [Required]
        [MaxLength(512)]
        public string Text { get; set; } = string.Empty;

        [Required]
        public DateTime SentAt { get; set; } = DateTime.Now;

        public DateTime? SeenAt { get; set; }
    }
}
