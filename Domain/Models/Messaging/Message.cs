using Domain.Models.Identity;
using Domain.Models.Orders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Messaging
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public ApplicationUser Sender { get; set; } = null!;

        [Required]
        public Guid ReceiverId { get; set; }
        [ForeignKey(nameof(ReceiverId))]
        public ApplicationUser Receiver { get; set; } = null!;

        [Required]
        public Guid ShippingOrderId { get; set; }
        [ForeignKey(nameof(ShippingOrderId))]
        public ShippingOrder ShippingOrder { get; set; } = null!;

        [Required]
        [MaxLength(512)]
        public string Text { get; set; } = string.Empty;

        [Required]
        public DateTime SentAt { get; set; }

        public DateTime? SeenAt { get; set; }
    }
}
