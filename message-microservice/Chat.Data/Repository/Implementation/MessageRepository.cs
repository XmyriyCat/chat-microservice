using AutoMapper;
using AutoMapper.QueryableExtensions;
using Chat.Data.Context;
using Chat.Data.Models;
using Chat.Data.Repository.Contracts;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Chat.Data.Repository.Implementation
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(ChatDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
        }

        public async Task<Result<IEnumerable<TDto>>> GetAllPaginationAsync<TDto>(int page, int pageSize, int senderUserId, int receiverUserId, Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy = null)
        {
            var validationResult = ValidateVariables(page, pageSize, senderUserId, receiverUserId);

            if (validationResult.IsFailed)
            {
                return validationResult;
            }

            var query = DataContext.Set<Message>()
                .Where(message => (message.Sender.Id == senderUserId && 
                                  message.Receiver.Id == receiverUserId) ||
                                  (message.Sender.Id == receiverUserId &&
                                   message.Receiver.Id == senderUserId))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<TDto>(Mapper.ConfigurationProvider);

            if (orderBy is not null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        private Result ValidateVariables(int page, int pageSize, int senderUserId, int receiverUserId)
        {
            if (page <= 0)
            {
                return Result.Fail($"The page should be greater than zero. Page = {page}");
            }

            if (pageSize <= 0)
            {
                return Result.Fail($"The page size should be greater than zero. Page size = {pageSize}");
            }

            if (senderUserId <= 0)
            {
                return Result.Fail($"The sender user id is less or equal to zero. Sender id = {senderUserId}");
            }

            if (receiverUserId <= 0)
            {
                return Result.Fail($"The receiver user id is less or equal to zero. Receiver id = {senderUserId}");
            }

            return Result.Ok();
        }
    }
}
