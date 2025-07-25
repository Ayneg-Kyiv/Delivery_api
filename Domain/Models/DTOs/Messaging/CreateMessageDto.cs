public class CreateMessageDto
{
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public Guid ShippingOrderId { get; set; }
    public string Text { get; set; } = string.Empty;
}
