namespace Application.DTOs.Orders
{
    public class UpdateShippingDestinationDto
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Street { get; set; }

        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}
