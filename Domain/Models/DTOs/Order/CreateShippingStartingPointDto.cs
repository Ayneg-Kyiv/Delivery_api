namespace Domain.Models.DTOs.Order
{
    public class CreateShippingStartingPointDto
    {
        public Guid ShippingOrderId { get; set; }

        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string HouseNumber { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AdditionalInfo { get; set; } = string.Empty;
    }
}
