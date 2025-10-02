namespace Domain.Models.DTOs.Support
{
    public class SupportChatRequest
    {
        public string UserMessage { get; set; } = string.Empty;
    }

    public class SupportChatResponse
    {
        public string Reply { get; set; } = string.Empty;
    }
}
