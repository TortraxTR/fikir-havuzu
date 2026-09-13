using api.Common;
using api.Dtos.Permission;
using api.Dtos.User;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _users;

        public UserController(IUserService users)
        {
            _users = users;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] Guid callerId)
        {
            var result = await _users.GetAllAsync(callerId);
            return result.ToActionResult();
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser([FromRoute] Guid id, [FromQuery] Guid callerId)
        {
            var result = await _users.GetByIdAsync(id, callerId);
            return result.ToActionResult();
        }

        // GET: api/users/{id}/permissions
        [HttpGet("{id}/permissions")]
        public async Task<ActionResult<IEnumerable<PermissionDto>>> GetUserPermissions([FromRoute] Guid id, [FromQuery] Guid callerId)
        {
            var result = await _users.GetPermissionsAsync(id, callerId);
            return result.ToActionResult();
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromQuery] Guid callerId, [FromBody] CreateUserRequestDto userDto)
        {
            var result = await _users.CreateAsync(callerId, userDto);
            if (!result.Ok)
            {
                return result.Error();
            }

            return CreatedAtAction(nameof(GetUser), new { id = result.Value!.Id, callerId }, result.Value);
        }

        // POST: api/users/{id}/permissions
        [HttpPost("{id}/permissions")]
        public async Task<ActionResult> AddPermissionToUser([FromRoute] Guid id, [FromQuery] Guid callerId, [FromBody] AddPermissionToUserRequestDto requestDto)
        {
            var result = await _users.AddPermissionAsync(id, callerId, requestDto.PermissionId);
            return result.ToActionResult();
        }

        // UPDATE: api/users/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser([FromRoute] Guid id, [FromQuery] Guid callerId, [FromBody] UpdateUserRequestDto userDto)
        {
            var result = await _users.UpdateAsync(id, callerId, userDto);
            return result.ToActionResult();
        }

        // UPDATE: api/users/{id}/setActive
        [HttpPut("{id}/setActive")]
        public async Task<ActionResult<UserDto>> SetUserActive([FromRoute] Guid id, [FromQuery] Guid callerId, [FromBody] SetUserActiveRequestDto requestDto)
        {
            var result = await _users.SetActiveAsync(id, callerId, requestDto.IsActive);
            return result.ToActionResult();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser([FromRoute] Guid id, [FromQuery] Guid callerId)
        {
            var result = await _users.DeleteAsync(id, callerId);
            return result.ToActionResult();
        }

        // DELETE: api/users/{id}/permissions/{permissionId}
        [HttpDelete("{id}/permissions/{permissionId}")]
        public async Task<ActionResult> RemovePermissionFromUser([FromRoute] Guid id, [FromRoute] Guid permissionId, [FromQuery] Guid callerId)
        {
            var result = await _users.RemovePermissionAsync(id, callerId, permissionId);
            return result.ToActionResult();
        }
    }
}
