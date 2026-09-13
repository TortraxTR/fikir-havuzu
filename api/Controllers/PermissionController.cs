using api.Common;
using api.Dtos.Permission;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/permissions")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissions;

        public PermissionController(IPermissionService permissions)
        {
            _permissions = permissions;
        }

        // GET: api/permissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionDto>>> GetPermissions([FromQuery] Guid callerId)
        {
            var result = await _permissions.GetAllAsync(callerId);
            return result.ToActionResult();
        }

        // GET: api/permissions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionDto>> GetPermission([FromRoute] Guid id, [FromQuery] Guid callerId)
        {
            var result = await _permissions.GetByIdAsync(id, callerId);
            return result.ToActionResult();
        }

        // POST: api/permissions
        [HttpPost]
        public async Task<ActionResult<PermissionDto>> CreatePermission([FromQuery] Guid callerId, [FromBody] CreatePermissionRequestDto permissionDto)
        {
            var result = await _permissions.CreateAsync(callerId, permissionDto);
            if (!result.Ok)
            {
                return result.Error();
            }

            return CreatedAtAction(nameof(GetPermission), new { id = result.Value!.Id, callerId }, result.Value);
        }

        // PUT: api/permissions/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<PermissionDto>> UpdatePermission([FromRoute] Guid id, [FromQuery] Guid callerId, [FromBody] UpdatePermissionRequestDto permissionDto)
        {
            var result = await _permissions.UpdateAsync(id, callerId, permissionDto);
            return result.ToActionResult();
        }

        // DELETE: api/permissions/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePermission([FromRoute] Guid id, [FromQuery] Guid callerId)
        {
            var result = await _permissions.DeleteAsync(id, callerId);
            return result.ToActionResult();
        }
    }
}
