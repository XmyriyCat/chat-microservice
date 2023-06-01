using AutoMapper;
using Chat.Data.Context;
using Chat.Data.Repository.Contracts;
using Chat.Data.Repository.Implementation;
using FluentResults;

namespace Chat.Data.UnitOfWork
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ChatDbContext _dataContext;

        private readonly IMapper _mapper;

        private IUserRepository _userRepository;

        private IMessageRepository _messageRepository;

        public RepositoryWrapper(ChatDbContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public IUserRepository Users
        {
            get
            {
                if (_userRepository is null)
                {
                    _userRepository = new UserRepository(_dataContext, _mapper);
                }

                return _userRepository;
            }
        }

        public IMessageRepository Messages
        {
            get
            {
                if (_messageRepository is null)
                {
                    _messageRepository = new MessageRepository(_dataContext, _mapper);
                }

                return _messageRepository;
            }
        }

        public async Task<Result> SaveChangesAsync()
        {
            try
            {
                await _dataContext.SaveChangesAsync();

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
