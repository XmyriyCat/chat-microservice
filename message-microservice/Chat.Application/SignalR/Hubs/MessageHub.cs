using Chat.Application.DTO.Message;
using Chat.Application.DTO.Page;
using Chat.Application.Services.Contract;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Application.SignalR.Hubs
{
    public class MessageHub : Hub<IMessageHub>
    {
        private readonly IUserCatalogService _userService;

        private readonly IMessageCatalogService _messageService;

        public MessageHub(IUserCatalogService userService, IMessageCatalogService messageService)
        {
            _userService = userService;
            _messageService = messageService;
        }

        public async Task SendFullMessage(CreateMessageDto message)
        {
            await _messageService.AddMessageAsync(message);

            var senderUserResult = await _userService.GetUserAsync(message.SenderUserId);

            var receiverUserResult = await _userService.GetUserAsync(message.ReceiverUserId);

            var messageOutput = new MessageOutputDto
            {
                SenderUsername = senderUserResult.ValueOrDefault.Username,
                ReceiverUsername = receiverUserResult.ValueOrDefault.Username,
                Value = message.Value
            };

            await Clients.All.ReceiveFullMessage(messageOutput);
        }

        public async Task SendChatHistoryRequest(GetMessagesRequestDto messageRequest)
        {
            var senderUser = await _userService.GetUserAsync(messageRequest.SenderUserId);

            var receiverUser = await _userService.GetUserAsync(messageRequest.ReceiverUserId);

            var paramPage = new PaginationParam();

            var messages = await _messageService.GetAllMessagesAsync(paramPage, senderUser.Value.Id, receiverUser.Value.Id);

            await Clients.All.ReceiveChatHistory(messages.Value);
        }
    }
}
