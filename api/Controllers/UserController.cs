using api.Authorization;
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
    private readonly IPermissionGuard _guard;

    public UserController(IUserRepository userRepository, IPermissionGuard guard)
    {
        _user_repo = userRepository;
        _guard = guard;
    }

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] Guid callerId)
    {
        var authorization = await _guard.RequireAsync(callerId, Permissions.BackOfficeRead);
        if (!authorization.Ok)
        {
            return authorization.ToActionResult();
        }

        var users = await _user_repo.GetAllUsersAsync();
        var usersDto = users.Select(u => u.ToUserDto());
        return Ok(usersDto);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser([FromRoute] Guid id, [FromQuery] Guid callerId)
    {
        if (callerId != id)
        {
            var authorization = await _guard.RequireAsync(callerId, Permissions.BackOfficeRead);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }
        }

        var user = await _user_repo.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user.ToUserDto());
    }

    // GET: api/users/{id}/permissions
    [HttpGet("{id}/permissions")]
    public async Task<ActionResult<IEnumerable<PermissionDto>>> GetUserPermissions([FromRoute] Guid id, [FromQuery] Guid callerId)
    {
        // A user may always read their own permissions (the landing page needs this);
        // reading someone else's requires a back-office permission.
        if (callerId != id)
        {
            var authorization = await _guard.RequireAsync(callerId, Permissions.BackOfficeRead);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }
        }

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
    public async Task<ActionResult<UserDto>> CreateUser([FromQuery] Guid callerId, [FromBody] Dtos.User.CreateUserRequestDto userDto)
    {
        var authorization = await _guard.RequireAsync(callerId, Permissions.UserManagement);
        if (!authorization.Ok)
        {
            return authorization.ToActionResult();
        }

        var user = userDto.ToUser();

        await _user_repo.CreateUserAsync(user);

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user.ToUserDto());
    }

    // POST: api/users/{id}/permissions
    [HttpPost("{id}/permissions")]
    public async Task<ActionResult> AddPermissionToUser([FromRoute] Guid id, [FromQuery] Guid callerId, [FromBody] Dtos.Permission.AddPermissionToUserRequestDto requestDto)
    {
        var authorization = await _guard.RequireAsync(callerId, Permissions.PermissionManagement);
        if (!authorization.Ok)
        {
            return authorization.ToActionResult();
        }

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
    public async Task<ActionResult<UserDto>> UpdateUser([FromRoute] Guid id, [FromQuery] Guid callerId, [FromBody] Dtos.User.UpdateUserRequestDto userDto)
    {
        var authorization = await _guard.RequireAsync(callerId, Permissions.UserManagement);
        if (!authorization.Ok)
        {
            return authorization.ToActionResult();
        }

        var updatedUser = await _user_repo.UpdateUserAsync(id, userDto);
        if (updatedUser == null)
        {
            return NotFound();
        }

        return Ok(updatedUser.ToUserDto());
    }

    // UPDATE: api/users/{id}/setActive
    [HttpPut("{id}/setActive")]
    public async Task<ActionResult<UserDto>> SetUserActive([FromRoute] Guid id, [FromQuery] Guid callerId, [FromBody] Dtos.User.SetUserActiveRequestDto requestDto)
    {
        var authorization = await _guard.RequireAsync(callerId, Permissions.UserManagement);
        if (!authorization.Ok)
        {
            return authorization.ToActionResult();
        }

        var updatedUser = await _user_repo.SetUserActiveAsync(id, requestDto.IsActive);
        if (updatedUser == null)
        {
            return NotFound();
        }

        return Ok(updatedUser.ToUserDto());
    }


    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser([FromRoute] Guid id, [FromQuery] Guid callerId)
    {
        var authorization = await _guard.RequireAsync(callerId, Permissions.UserManagement);
        if (!authorization.Ok)
        {
            return authorization.ToActionResult();
        }

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
    public async Task<ActionResult> RemovePermissionFromUser([FromRoute] Guid id, [FromRoute] Guid permissionId, [FromQuery] Guid callerId)
    {
        var authorization = await _guard.RequireAsync(callerId, Permissions.PermissionManagement);
        if (!authorization.Ok)
        {
            return authorization.ToActionResult();
        }

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
