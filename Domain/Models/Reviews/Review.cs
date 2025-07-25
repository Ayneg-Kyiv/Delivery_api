using Domain.Models.Identity;
using Domain.Models.Orders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Reviews
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public Guid ShippingOrderId { get; set; }
        [ForeignKey(nameof(ShippingOrderId))]
        public ShippingOrder ShippingOrder { get; set; } = null!;

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [MaxLength(512)]
        public string Text { get; set; } = string.Empty;
    }
}
