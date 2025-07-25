using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Orders
{
    public class ShippingStartingPoint
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime LastUpdatedAt { get; set; }

        [Required]
        public Guid ShippingOrderId { get; set; }

        [ForeignKey(nameof(ShippingOrderId))]
        public ShippingOrder ShippingOrder { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? District { get; set; }

        [Required]
        [MaxLength(200)]
        public string Street { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? HouseNumber { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public string? AdditionalInfo { get; set; }
    }
}