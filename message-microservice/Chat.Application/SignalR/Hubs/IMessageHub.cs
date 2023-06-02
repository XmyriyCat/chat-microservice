using Chat.Application.DTO.Message;

namespace Chat.Application.SignalR.Hubs
{
    public interface IMessageHub
    {
        Task ReceiveMessage(CreateMessageDto message);
    }
}
