using Chat.Application.DTO.Page;
using Chat.Application.DTO.User;
using Chat.Data.Models;
using FluentResults;

namespace Chat.Application.Services.Contract
{
    public interface IUserCatalogService
    {
        Task<Result<IEnumerable<UserOutputDto>>> GetAllUsersAsync(PaginationParam pageParam);

        Task<Result<User>> GetUserAsync(int id);

        Task<Result<UserOutputDto>> AddUserAsync(CreateUserDto userDto);

        Task<Result> DeleteUserAsync(int id);
    }
}
