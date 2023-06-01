using FluentResults;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Chat.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Chat.Data.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly ChatDbContext DataContext;

        protected readonly IMapper Mapper;

        public GenericRepository(ChatDbContext dataContext, IMapper mapper)
        {
            DataContext = dataContext;
            Mapper = mapper;
        }

        public virtual async Task<IEnumerable<TDto>> GetAllAsync<TDto>(Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy = null)
        {
            var query = DataContext.Set<T>()
                .ProjectTo<TDto>(Mapper.ConfigurationProvider);

            if (orderBy is not null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<Result<IEnumerable<TDto>>> GetAllPaginationAsync<TDto>(int page, int pageSize,
            Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy = null)
        {
            if (page <= 0)
            {
                return new Result<IEnumerable<TDto>>().WithError($"The page should be greater than zero. Page = {page}");
            }

            if (pageSize <= 0)
            {
                return new Result<IEnumerable<TDto>>().WithError($"The page size should be greater than zero. Page size = {pageSize}");
            }

            var query = DataContext.Set<T>()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<TDto>(Mapper.ConfigurationProvider);

            if (orderBy is not null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<T> AddAsync(T item)
        {
            var creationResult = await DataContext.Set<T>()
                .AddAsync(item);

            return creationResult.Entity;
        }

        public virtual T Update(T item)
        {
            var updateResult = DataContext.Set<T>().Update(item);

            return updateResult.Entity;
        }

        public virtual void Delete(T item)
        {
            DataContext.Set<T>().Remove(item);
        }

        public virtual async Task<int> CountAsync()
        {
            return await DataContext.Set<T>().CountAsync();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await DataContext.Set<T>().AnyAsync(predicate);
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await DataContext.Set<T>().FirstOrDefaultAsync(expression);
        }
    }
}
