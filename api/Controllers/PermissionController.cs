using api.Interfaces;
using api.Dtos.Permission;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/permissions")]
    [ApiController]

    public class PermissionController : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionController(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        // GET: api/permissions
        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            var permissions = await _permissionRepository.GetAllPermissionsAsync();
            return Ok(permissions);
        }

        // GET: api/permissions/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPermission([FromRoute] Guid id)
        {
            var permission = await _permissionRepository.GetPermissionByIdAsync(id);
            if (permission == null)
            {
                return NotFound();
            }
            return Ok(permission);
        }

        // POST: api/permissions
        [HttpPost]
        public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionRequestDto permissionDto)
        {
            var permission = new Permission
            {
                Name = permissionDto.Name
            };

            var createdPermission = await _permissionRepository.CreatePermissionAsync(permission);
            return CreatedAtAction(nameof(GetPermission), new { id = createdPermission.Id }, createdPermission);
        }

        // PUT: api/permissions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermission([FromRoute] Guid id, [FromBody] UpdatePermissionRequestDto permissionDto)
        {
            var permission = new Permission
            {
                Name = permissionDto.Name
            };

            var updatedPermission = await _permissionRepository.UpdatePermissionAsync(id, permission);
            if (updatedPermission == null)
            {
                return NotFound();
            }

            return Ok(updatedPermission);
        }

        // DELETE: api/permissions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission([FromRoute] Guid id)
        {
            var deleted = await _permissionRepository.DeletePermissionAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}