using Domain.Models.Orders;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Messaging
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        // Логічні зв'язки з ApplicationUser через ID (без FK)
        [Required]
        public Guid SenderId { get; set; }

        [Required]
        public Guid ReceiverId { get; set; }

        [Required]
        public Guid ShippingOrderId { get; set; }
        public ShippingOrder ShippingOrder { get; set; } = null!;

        [Required]
        [MaxLength(512)]
        public string Text { get; set; } = string.Empty;

        [Required]
        public DateTime SentAt { get; set; }

        public DateTime? SeenAt { get; set; }
    }
}
