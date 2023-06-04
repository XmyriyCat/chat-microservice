using Chat.Data.Models;
using FluentResults;

namespace Chat.Data.Repository.Contracts
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<Result<IEnumerable<TDto>>> GetAllPaginationAsync<TDto>(
            int page,
            int pageSize,
            int senderUserId,
            int receiverUserId,
            Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy = null);
    }
}
