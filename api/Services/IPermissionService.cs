using api.Common;
using api.Dtos.Permission;

namespace api.Services
{
    public interface IPermissionService
    {
        Task<Result<IEnumerable<PermissionDto>>> GetAllAsync(Guid callerId);

        Task<Result<PermissionDto>> GetByIdAsync(Guid id, Guid callerId);

        Task<Result<PermissionDto>> CreateAsync(Guid callerId, CreatePermissionRequestDto dto);

        Task<Result<PermissionDto>> UpdateAsync(Guid id, Guid callerId, UpdatePermissionRequestDto dto);

        Task<Result> DeleteAsync(Guid id, Guid callerId);
    }
}
