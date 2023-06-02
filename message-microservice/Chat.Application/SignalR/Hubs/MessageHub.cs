using Chat.Application.DTO.Message;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Application.SignalR.Hubs
{
    public class MessageHub : Hub<IMessageHub>
    {
        public async Task SendMessage(CreateMessageDto message)
        {
            await Clients.All.ReceiveMessage(message);
        }
    }
}
