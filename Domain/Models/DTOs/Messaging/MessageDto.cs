public class MessageDto
{
    public int Id { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public Guid ShippingOrderId { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public DateTime? SeenAt { get; set; }
}
