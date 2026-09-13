using api.Common;
using api.Dtos.Permission;
using api.Dtos.User;

namespace api.Services
{
    public interface IUserService
    {
        Task<Result<IEnumerable<UserDto>>> GetAllAsync(Guid callerId);

        Task<Result<UserDto>> GetByIdAsync(Guid id, Guid callerId);

        Task<Result<IEnumerable<PermissionDto>>> GetPermissionsAsync(Guid id, Guid callerId);

        Task<Result<UserDto>> CreateAsync(Guid callerId, CreateUserRequestDto dto);

        Task<Result<UserDto>> UpdateAsync(Guid id, Guid callerId, UpdateUserRequestDto dto);

        Task<Result<UserDto>> SetActiveAsync(Guid id, Guid callerId, bool isActive);

        Task<Result> DeleteAsync(Guid id, Guid callerId);

        Task<Result> AddPermissionAsync(Guid id, Guid callerId, Guid permissionId);

        Task<Result> RemovePermissionAsync(Guid id, Guid callerId, Guid permissionId);
    }
}
