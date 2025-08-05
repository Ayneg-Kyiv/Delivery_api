namespace Domain.Models.DTOs.Order
{
    public class UpdateShippingOfferDto
    {
        public decimal? OfferedPrice { get; set; }
        public DateOnly? OfferedDate { get; set; }
        public TimeOnly? OfferedTime { get; set; }

        public bool? IsAccepted { get; set; }
        public string? Comment { get; set; }
    }
}
