using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Orders
{
    public class ShippingDestination
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime LastUpdatedAt { get; set; }

        [Required]
        public Guid ShippingOrderId { get; set; }

        // Navigation property для FK зв'язку з ShippingOrder
        public ShippingOrder? ShippingOrder { get; set; }

        [Required] public string Country { get; set; } = string.Empty;
        [Required] public string City { get; set; } = string.Empty;
        [Required] public string District { get; set; } = string.Empty;
        [Required] public string Street { get; set; } = string.Empty;

        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string MiddleName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;

        [Required] public string PhoneNumber { get; set; } = string.Empty;
        [Required] public string AdditionalInfo { get; set; } = string.Empty;
    }
}
