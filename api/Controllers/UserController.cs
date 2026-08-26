using api.Interfaces;
using api.Dtos.User;
using api.Dtos.Permission;
using api.Mappers.UserMappers;
using Microsoft.AspNetCore.Mvc;
using api.Mappers.PermissionMappers;
using api.Models;

namespace api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
{
    private readonly IUserRepository _user_repo;
    public UserController(IUserRepository userRepository)
    {
        _user_repo = userRepository;

    }
    
    // GET: api/users
    [HttpGet]
    
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _user_repo.GetAllUsersAsync();
        var usersDto = users.Select(u => u.ToUserDto());
        return Ok(usersDto);
    }
    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser([FromRoute] Guid id)
    {
        var user = await _user_repo.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user.ToUserDto());
    }
    
    // GET: api/users/{id}/permissions
    [HttpGet("{id}/permissions")]

    public async Task<ActionResult<IEnumerable<PermissionDto>>> GetUserPermissions([FromRoute] Guid id)
    {
        var user = await _user_repo.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var permissions = await _user_repo.GetUserPermissionsAsync(id);
        var permissionsDto = permissions.Select(p => p.ToPermissionDto());
        return Ok(permissionsDto);
    }

    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] Dtos.User.CreateUserRequestDto userDto)
    {
        var user = userDto.ToUser();

        await _user_repo.CreateUserAsync(user);

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user.ToUserDto());
    }

    // POST: api/users/{id}/permissions
    [HttpPost("{id}/permissions")]
    public async Task<ActionResult> AddPermissionToUser([FromRoute] Guid id, [FromBody] Dtos.Permission.AddPermissionToUserRequestDto requestDto)
    {
        var user = await _user_repo.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var success = await _user_repo.AddPermissionToUserAsync(id, requestDto.PermissionId);
        if (!success)
        {
            return BadRequest("Failed to add permission to user.");
        }

        return NoContent();
    }

    // UPDATE: api/users/{id}
    [HttpPut("{id}")]

    public async Task<ActionResult<UserDto>> UpdateUser([FromRoute] Guid id, [FromBody] Dtos.User.UpdateUserRequestDto userDto)
    {
        var updatedUser = await _user_repo.UpdateUserAsync(id, userDto);
        if (updatedUser == null)
        {
            return NotFound();
        }

        return Ok(updatedUser.ToUserDto());
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser([FromRoute] Guid id)
    {
        var user = await _user_repo.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        await _user_repo.DeleteUserAsync(id);

        return NoContent();
    }

    // DELETE: api/users/{id}/permissions/{permissionId}
    [HttpDelete("{id}/permissions/{permissionId}")]
    public async Task<ActionResult> RemovePermissionFromUser([FromRoute] Guid id, [FromRoute] Guid permissionId)
    {
        var user = await _user_repo.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var success = await _user_repo.RemovePermissionFromUserAsync(id, permissionId);
        if (!success)
        {
            return BadRequest("Failed to remove permission from user.");
        }

        return NoContent();
    }

}};