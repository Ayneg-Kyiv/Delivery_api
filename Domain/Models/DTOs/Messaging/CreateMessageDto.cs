public class CreateMessageDto
{
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public Guid? DeliveryOrderId { get; set; }
    public Guid? DeliveryOfferId { get; set; }
    public string Text { get; set; } = string.Empty;
}
