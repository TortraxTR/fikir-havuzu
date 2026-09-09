using api.Authorization;
using api.Interfaces;
using api.Dtos.Permission;
using api.Models;
using api.Mappers.PermissionMappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/permissions")]
    [ApiController]

    public class PermissionController : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionGuard _guard;

        public PermissionController(IPermissionRepository permissionRepository, IPermissionGuard guard)
        {
            _permissionRepository = permissionRepository;
            _guard = guard;
        }

        // GET: api/permissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionDto>>> GetPermissions([FromQuery] Guid callerId)
        {
            var authorization = await _guard.RequireAsync(callerId, Permissions.BackOfficeRead);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var permissions = await _permissionRepository.GetAllPermissionsAsync();
            return Ok(permissions.Select(p => p.ToPermissionDto()));
        }

        // GET: api/permissions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionDto>> GetPermission([FromRoute] Guid id, [FromQuery] Guid callerId)
        {
            var authorization = await _guard.RequireAsync(callerId, Permissions.BackOfficeRead);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var permission = await _permissionRepository.GetPermissionByIdAsync(id);
            if (permission == null)
            {
                return NotFound();
            }
            return Ok(permission.ToPermissionDto());
        }

        // POST: api/permissions
        [HttpPost]
        public async Task<ActionResult<PermissionDto>> CreatePermission([FromQuery] Guid callerId, [FromBody] CreatePermissionRequestDto permissionDto)
        {
            var authorization = await _guard.RequireAsync(callerId, Permissions.UserManagement);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var permission = new Permission
            {
                Code = permissionDto.Code,
                Name = permissionDto.Name
            };

            var createdPermission = await _permissionRepository.CreatePermissionAsync(permission);
            return CreatedAtAction(nameof(GetPermission), new { id = createdPermission.Id }, createdPermission.ToPermissionDto());
        }

        // PUT: api/permissions/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<PermissionDto>> UpdatePermission([FromRoute] Guid id, [FromQuery] Guid callerId, [FromBody] UpdatePermissionRequestDto permissionDto)
        {
            var authorization = await _guard.RequireAsync(callerId, Permissions.UserManagement);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var permission = new Permission
            {
                Code = permissionDto.Code,
                Name = permissionDto.Name
            };

            var updatedPermission = await _permissionRepository.UpdatePermissionAsync(id, permission);
            if (updatedPermission == null)
            {
                return NotFound();
            }

            return Ok(updatedPermission.ToPermissionDto());
        }

        // DELETE: api/permissions/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePermission([FromRoute] Guid id, [FromQuery] Guid callerId)
        {
            var authorization = await _guard.RequireAsync(callerId, Permissions.UserManagement);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var deleted = await _permissionRepository.DeletePermissionAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
