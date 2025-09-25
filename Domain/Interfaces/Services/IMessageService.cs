using Domain.Models.DTOs;

namespace Domain.Interfaces.Services
{
    public interface IMessageService
    {
        Task<TResponse> SendAsync(CreateMessageDto message, CancellationToken cancellationToken);
        Task<TResponse> GetMessagesByOrder(Guid orderId, CancellationToken cancellationToken);
        Task<TResponse> GetMessagesByOffer(Guid offerId, CancellationToken cancellationToken);
        Task<TResponse> MarkAsSeenAsync(Guid messageId, CancellationToken cancellationToken);
    }
}
