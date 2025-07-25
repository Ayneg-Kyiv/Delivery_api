public class ReviewDto
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string Text { get; set; } = string.Empty;
    public int ShippingOrderId { get; set; }
    public Guid UserId { get; set; }
}
