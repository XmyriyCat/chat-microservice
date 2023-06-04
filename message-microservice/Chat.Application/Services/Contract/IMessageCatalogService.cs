using Chat.Application.DTO.Message;
using Chat.Application.DTO.Page;
using Chat.Data.Models;
using FluentResults;

namespace Chat.Application.Services.Contract
{
    public interface IMessageCatalogService
    {
        Task<Result<IEnumerable<MessageOutputDto>>> GetAllMessagesAsync(PaginationParam pageParam);

        Task<Result<IEnumerable<MessageOutputDto>>> GetAllMessagesAsync(PaginationParam pageParam, int senderUserId, int receiverUserId);

        Task<Result<Message>> GetMessageAsync(int id);

        Task<Result<MessageOutputDto>> AddMessageAsync(CreateMessageDto messageDto);

        Task<Result> DeleteMessageAsync(int id);
    }
}
