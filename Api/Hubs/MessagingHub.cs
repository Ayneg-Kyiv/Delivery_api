using Domain.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs
{
    public class MessagingHub(IMessageService messageService) : Hub
    {

        // Join a room for order-based messaging
        public async Task JoinOrderRoom(Guid orderId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"order_{orderId}");

            // Fetch message history and send it to the client
            var messagesResponse = await messageService.GetMessagesByOrder(orderId, CancellationToken.None);
            if (messagesResponse.Success)
            {
                await Clients.Caller.SendAsync("ReceiveMessageHistory", messagesResponse.Data);
            }
        }

        // Join a room for offer-based messaging
        public async Task JoinOfferRoom(Guid offerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"offer_{offerId}");

            // Fetch message history and send it to the client
            var messagesResponse = await messageService.GetMessagesByOffer(offerId, CancellationToken.None);
            if (messagesResponse.Success)
            {
                await Clients.Caller.SendAsync("ReceiveMessageHistory", messagesResponse.Data);
            }
        }

        // Leave a room for order-based messaging
        public async Task LeaveOrderRoom(Guid orderId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"order_{orderId}");
        }

        // Leave a room for offer-based messaging
        public async Task LeaveOfferRoom(Guid offerId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"offer_{offerId}");
        }

        // Send a message within an order room
        public async Task SendMessageToOrder(CreateMessageDto message)
        {
            // Save the message to the database
            var response = await messageService.SendAsync(message, CancellationToken.None);

            if (response.Success)
            {
                // Broadcast the message to the room
                await Clients.Group($"order_{message.DeliveryOrderId}").SendAsync("ReceiveMessage", response.Data);
            }
        }

        // Send a message within an offer room
        public async Task SendMessageToOffer(CreateMessageDto message)
        {
            // Save the message to the database
            var response = await messageService.SendAsync(message, CancellationToken.None);

            if (response.Success)
            {
                // Broadcast the message to the room
                await Clients.Group($"offer_{message.DeliveryOfferId}").SendAsync("ReceiveMessage", response.Data);
            }
        }

        // Mark message as seen
        public async Task MarkMessageAsSeen(Guid messageId, Guid roomId, bool isOrderRoom)
        {
            var response = await messageService.MarkAsSeenAsync(messageId, CancellationToken.None);

            if (response.Success)
            {
                // Notify the room that the message has been seen
                string roomType = isOrderRoom ? "order" : "offer";
                await Clients.Group($"{roomType}_{roomId}").SendAsync("MessageSeen", messageId);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Any cleanup needed when a user disconnects
            await base.OnDisconnectedAsync(exception);
        }
    }
}