using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Orders
{
    public class ShippingObject
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime LastUpdatedAt { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public float Weight { get; set; }

        [Required]
        public float Width { get; set; }

        [Required]
        public float Height { get; set; }

        [Required]
        public float Length { get; set; }

        [Required]
        public Guid ShippingOrderId { get; set; }

        [ForeignKey(nameof(ShippingOrderId))]
        public ShippingOrder ShippingOrder { get; set; } = null!;

        public string? ImagePath { get; set; }
    }
}
