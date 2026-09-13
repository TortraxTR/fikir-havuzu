using api.Authorization;
using api.Common;
using api.Dtos.Permission;
using api.Interfaces;
using api.Mappers.PermissionMappers;
using api.Models;

namespace api.Services
{
    public sealed class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissions;
        private readonly IPermissionChecker _access;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IPermissionRepository permissions, IPermissionChecker access, IUnitOfWork unitOfWork)
        {
            _permissions = permissions;
            _access = access;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<PermissionDto>>> GetAllAsync(Guid callerId)
        {
            var access = await _access.CheckAsync(callerId, Permissions.BackOfficeRead);
            if (!access.Granted)
            {
                return access.ToFailure<IEnumerable<PermissionDto>>();
            }

            var permissions = await _permissions.GetAllPermissionsAsync();
            return Result<IEnumerable<PermissionDto>>.Success(permissions.Select(p => p.ToPermissionDto()));
        }

        public async Task<Result<PermissionDto>> GetByIdAsync(Guid id, Guid callerId)
        {
            var access = await _access.CheckAsync(callerId, Permissions.BackOfficeRead);
            if (!access.Granted)
            {
                return access.ToFailure<PermissionDto>();
            }

            var permission = await _permissions.GetPermissionByIdAsync(id);
            return permission == null
                ? Result<PermissionDto>.Fail(ResultError.NotFound)
                : Result<PermissionDto>.Success(permission.ToPermissionDto());
        }

        public async Task<Result<PermissionDto>> CreateAsync(Guid callerId, CreatePermissionRequestDto dto)
        {
            var access = await _access.CheckAsync(callerId, Permissions.UserManagement);
            if (!access.Granted)
            {
                return access.ToFailure<PermissionDto>();
            }

            var permission = new Permission { Code = dto.Code, Name = dto.Name };
            var created = await _permissions.CreatePermissionAsync(permission);
            await _unitOfWork.SaveChangesAsync();
            return Result<PermissionDto>.Success(created.ToPermissionDto());
        }

        public async Task<Result<PermissionDto>> UpdateAsync(Guid id, Guid callerId, UpdatePermissionRequestDto dto)
        {
            var access = await _access.CheckAsync(callerId, Permissions.UserManagement);
            if (!access.Granted)
            {
                return access.ToFailure<PermissionDto>();
            }

            var permission = new Permission { Code = dto.Code, Name = dto.Name };
            var updated = await _permissions.UpdatePermissionAsync(id, permission);
            if (updated == null)
            {
                return Result<PermissionDto>.Fail(ResultError.NotFound);
            }

            await _unitOfWork.SaveChangesAsync();
            return Result<PermissionDto>.Success(updated.ToPermissionDto());
        }

        public async Task<Result> DeleteAsync(Guid id, Guid callerId)
        {
            var access = await _access.CheckAsync(callerId, Permissions.UserManagement);
            if (!access.Granted)
            {
                return access.ToFailure();
            }

            var deleted = await _permissions.DeletePermissionAsync(id);
            if (!deleted)
            {
                return Result.Fail(ResultError.NotFound);
            }

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
