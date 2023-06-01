using Chat.Data.Repository.Contracts;
using FluentResults;

namespace Chat.Data.UnitOfWork
{
    public interface IRepositoryWrapper
    {
        IUserRepository Users { get; }

        IMessageRepository Messages { get; }

        Task<Result> SaveChangesAsync();
    }
}
