using AutoMapper;
using Chat.Data.Context;
using Chat.Data.Models;
using Chat.Data.Repository.Contracts;

namespace Chat.Data.Repository.Implementation
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ChatDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
        }
    }
}
