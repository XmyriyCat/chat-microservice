using System.Linq.Expressions;
using FluentResults;

namespace Chat.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<TDto>> GetAllAsync<TDto>(Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy = null);

        Task<Result<IEnumerable<TDto>>> GetAllPaginationAsync<TDto>(int page, int pageSize,
            Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy = null);

        Task<T> AddAsync(T item);

        T Update(T item);

        void Delete(T item);

        Task<int> CountAsync();

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}
