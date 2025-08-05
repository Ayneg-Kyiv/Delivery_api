using Domain.Models.Orders;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Reviews
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        // Логічний зв'язок з ApplicationUser через ID (без FK)
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ShippingOrderId { get; set; }
        public ShippingOrder ShippingOrder { get; set; } = null!;

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [MaxLength(512)]
        public string Text { get; set; } = string.Empty;
    }
}
