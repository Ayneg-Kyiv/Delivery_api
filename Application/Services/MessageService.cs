using AutoMapper;
using Domain.Interfaces.Services;
using Domain.Models.DTOs;
using Domain.Models.Messaging;
using Infrastructure.Contexts;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class MessageService(
        IBaseRepository<Message, ShippingDbContext> messagesRepository,
        IMapper mapper
    ) : IMessageService
    {
        public async Task<TResponse> SendAsync(CreateMessageDto message, CancellationToken cancellationToken)
        {
            if (message == null)
                return TResponse.Failure(400, "Message cannot be null");

            var newMessage = mapper.Map<Message>(message);

            var result = await messagesRepository.AddAsync(newMessage, cancellationToken);

            if (!result)
                return TResponse.Failure(500, "Failed to send message");

            return TResponse.Successful(mapper.Map<MessageDto>(newMessage), "Message sent successfully");
        }

        public async Task<TResponse> GetMessagesByOffer(Guid offerId, CancellationToken cancellationToken)
        {
            var message = await messagesRepository.FindAsync([m => m.DeliveryOfferId == offerId], cancellationToken);

            var messagesDtos = mapper.Map<IEnumerable<MessageDto>>(message);

            return TResponse.Successful(message, "Messages retrieved successfully");
        }

        public async Task<TResponse> GetMessagesByOrder(Guid orderId, CancellationToken cancellationToken)
        {
            var message = await messagesRepository.FindAsync([m => m.DeliveryOrderId == orderId], cancellationToken);

            var messagesDtos = mapper.Map<IEnumerable<MessageDto>>(message);

            return TResponse.Successful(message, "Messages retrieved successfully");
        }

        public async Task<TResponse> MarkAsSeenAsync(Guid messageId, CancellationToken cancellationToken)
        {
            var messages = await messagesRepository.FindAsync([m => m.Id == messageId], cancellationToken);

            if (messages == null || !messages.Any())
                return TResponse.Failure(404, "Message not found");

            var message = messages.FirstOrDefault();

            if (message == null)
                return TResponse.Failure(400, "Message already marked as seen");

            message.SeenAt = DateTime.UtcNow;

            var result = await messagesRepository.UpdateAsync(message, cancellationToken);

            if (!result)
                return TResponse.Failure(500, "Failed to update message");

            return TResponse.Successful(result, "Message marked as seen");
        }
    }
}
