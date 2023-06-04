using AutoMapper;
using Chat.Application.DTO.Message;
using Chat.Application.DTO.Page;
using Chat.Application.Services.Contract;
using Chat.Data.Models;
using Chat.Data.UnitOfWork;
using FluentResults;

namespace Chat.Application.Services.Implementation
{
    public class MessageCatalogService : IMessageCatalogService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        private readonly IMapper _mapper;

        public MessageCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<MessageOutputDto>>> GetAllMessagesAsync(PaginationParam pageParam)
        {
            var messagesDtoResult = await _repositoryWrapper.Messages.GetAllPaginationAsync<MessageOutputDto>
            (
                pageParam.Page,
                pageParam.PageSize,
                messages => messages.OrderBy(message => message.Id)
            );

            return messagesDtoResult;
        }

        public async Task<Result<IEnumerable<MessageOutputDto>>> GetAllMessagesAsync(PaginationParam pageParam, int senderUserId, int receiverUserId)
        {
            var messagesDtoResult = await _repositoryWrapper.Messages.GetAllPaginationAsync<MessageOutputDto>
            (
                page: pageParam.Page,
                pageSize: pageParam.PageSize,
                senderUserId: senderUserId,
                receiverUserId: receiverUserId,
                orderBy: messages => messages.OrderBy(message => message.Id)
            );

            return messagesDtoResult;
        }

        public async Task<Result<Message>> GetMessageAsync(int id)
        {
            var message = await _repositoryWrapper.Messages.FirstOrDefaultAsync(message => message.Id == id);

            if (message is null)
            {
                return new Result<Message>().WithError($"Message with id = {id} is not found in database");
            }

            return message;
        }

        public async Task<Result<MessageOutputDto>> AddMessageAsync(CreateMessageDto messageDto)
        {
            var mappingResult = await TryMapWithTrackingAsync(messageDto);

            if (mappingResult.IsFailed)
            {
                return Result.Fail(mappingResult.Errors);
            }

            var message = mappingResult.Value;
            
            var createdMessage = await _repositoryWrapper.Messages.AddAsync(message);

            var saveResult = await _repositoryWrapper.SaveChangesAsync();

            if (saveResult.IsFailed)
            {
                return new Result<MessageOutputDto>().WithErrors(saveResult.Errors);
            }

            var messageOutputDto = _mapper.Map<MessageOutputDto>(createdMessage);

            return messageOutputDto;
        }

        public async Task<Result> DeleteMessageAsync(int id)
        {
            var getMessageDbResult = await GetMessageAsync(id);

            if (getMessageDbResult.IsFailed)
            {
                return Result.Fail(getMessageDbResult.Errors);
            }

            var messageDb = getMessageDbResult.Value;

            _repositoryWrapper.Messages.Delete(messageDb);

            var saveResult = await _repositoryWrapper.SaveChangesAsync();

            return saveResult;
        }

        private async Task<Result<Message>> TryMapWithTrackingAsync(CreateMessageDto messageDto)
        {
            var senderUser = await _repositoryWrapper.Users.FirstOrDefaultAsync(user => user.Id == messageDto.SenderUserId);

            if (senderUser is null)
            {
                return Result.Fail($"Sender user id is not found in database. User id = {messageDto.SenderUserId}");
            }

            var receiverUser = await _repositoryWrapper.Users.FirstOrDefaultAsync(user => user.Id == messageDto.ReceiverUserId);

            if (receiverUser is null)
            {
                return Result.Fail($"Receiver user id is not found in database. User id = {messageDto.ReceiverUserId}");
            }

            var trackingMessage = _mapper.Map<Message>(messageDto);
            trackingMessage.Sender = senderUser;
            trackingMessage.Receiver = receiverUser;

            return trackingMessage;
        }
    }
}
