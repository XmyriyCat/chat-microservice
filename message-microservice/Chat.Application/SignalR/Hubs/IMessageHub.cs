using Chat.Application.DTO.Message;

namespace Chat.Application.SignalR.Hubs
{
    public interface IMessageHub
    {
        Task ReceiveFullMessage(MessageOutputDto message);

        Task ReceiveChatHistory(IEnumerable<MessageOutputDto> messages);
    }
}
