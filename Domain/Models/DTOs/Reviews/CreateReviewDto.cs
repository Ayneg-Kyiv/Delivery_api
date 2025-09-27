public class CreateReviewDto
{
    public int Rating { get; set; }
    public string Text { get; set; } = string.Empty;
    public Guid? ShippingOrderId { get; set; }

    public Guid? DeliveryOrderId { get; set; }

    public Guid? DeliveryRequestId { get; set; }

    public Guid? ReviewerId { get; set; }
    public Guid? UserId { get; set; }
}
