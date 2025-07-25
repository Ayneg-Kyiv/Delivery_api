namespace Application.DTOs.Orders
{
    public class CreateShippingDestinationDto
    {
        public Guid ShippingOrderId { get; set; }

        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
        public string AdditionalInfo { get; set; } = string.Empty;
    }
}
