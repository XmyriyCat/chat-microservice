using AutoMapper;
using Chat.Application.DTO.Page;
using Chat.Application.DTO.User;
using Chat.Application.Services.Contract;
using Chat.Data.Models;
using Chat.Data.UnitOfWork;
using FluentResults;

namespace Chat.Application.Services.Implementation
{
    public class UserCatalogService : IUserCatalogService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        private readonly IMapper _mapper;

        public UserCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<UserOutputDto>>> GetAllUsersAsync(PaginationParam pageParam)
        {
            var usersDtoResult = await _repositoryWrapper.Users.GetAllPaginationAsync<UserOutputDto>
            (
                pageParam.Page,
                pageParam.PageSize,
                user => user.OrderBy(prop => prop.Id)
            );

            return usersDtoResult;
        }

        public async Task<Result<User>> GetUserAsync(int id)
        {
            var user = await _repositoryWrapper.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (user is null)
            {
                return new Result<User>().WithError($"User with id = {id} is not found in database");
            }

            return user;
        }

        public async Task<Result<UserOutputDto>> AddUserAsync(CreateUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            var createdUser = await _repositoryWrapper.Users.AddAsync(user);

            var saveResult = await _repositoryWrapper.SaveChangesAsync();

            if (saveResult.IsFailed)
            {
                return new Result<UserOutputDto>().WithErrors(saveResult.Errors);
            }

            var userOutputDto = _mapper.Map<UserOutputDto>(createdUser);

            return userOutputDto;
        }

        public async Task<Result> DeleteUserAsync(int id)
        {
            var getUserDbResult = await GetUserAsync(id);

            if (getUserDbResult.IsFailed)
            {
                return Result.Fail(getUserDbResult.Errors);
            }

            var userDb = getUserDbResult.Value;

            _repositoryWrapper.Users.Delete(userDb);

            var saveResult = await _repositoryWrapper.SaveChangesAsync();

            return saveResult;
        }
    }
}
