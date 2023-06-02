using Chat.Application.DTO.Page;
using Chat.Application.DTO.User;
using Chat.Application.Services.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Chat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserCatalogService _userCatalogService;

        public UsersController(IUserCatalogService userCatalogService)
        {
            _userCatalogService = userCatalogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] PaginationParam pageParam)
        {
            var usersResult = await _userCatalogService.GetAllUsersAsync(pageParam);

            if (usersResult.IsFailed)
            {
                return BadRequest(string.Join(", ", usersResult.Errors.Select(error => error.Message)));
            }

            return Ok(usersResult.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync([FromRoute] int id)
        {
            var userResult = await _userCatalogService.GetUserAsync(id);

            if (userResult.IsFailed)
            {
                return BadRequest(string.Join(", ", userResult.Errors.Select(error => error.Message)));
            }

            return Ok(userResult.Value);
        }

        [HttpPost, ActionName(nameof(CreateUserAsync))]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDto userDto)
        {
            var creationResult = await _userCatalogService.AddUserAsync(userDto);

            if (creationResult.IsFailed)
            {
                return BadRequest(string.Join(", ", creationResult.Errors.Select(error => error.Message)));
            }

            return CreatedAtAction(nameof(CreateUserAsync), creationResult.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var deletionResult = await _userCatalogService.DeleteUserAsync(id);

            if (deletionResult.IsFailed)
            {
                return NotFound(string.Join(", ", deletionResult.Errors.Select(error => error.Message)));
            }

            return Ok(deletionResult);
        }
    }
}
