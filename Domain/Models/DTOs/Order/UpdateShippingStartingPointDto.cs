namespace Domain.Models.DTOs.Order
{
    public class UpdateShippingStartingPointDto
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}
