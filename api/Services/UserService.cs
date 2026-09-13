using api.Authorization;
using api.Common;
using api.Dtos.Permission;
using api.Dtos.User;
using api.Interfaces;
using api.Mappers.PermissionMappers;
using api.Mappers.UserMappers;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace api.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _users;
        private readonly IPermissionChecker _permissions;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository users, IPermissionChecker permissions, IUnitOfWork unitOfWork)
        {
            _users = users;
            _permissions = permissions;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<UserDto>>> GetAllAsync(Guid callerId)
        {
            var access = await _permissions.CheckAsync(callerId, Permissions.BackOfficeRead);
            if (!access.Granted)
            {
                return access.ToFailure<IEnumerable<UserDto>>();
            }

            var users = await _users.GetAllUsersAsync();
            return Result<IEnumerable<UserDto>>.Success(users.Select(u => u.ToUserDto()));
        }

        public async Task<Result<UserDto>> GetByIdAsync(Guid id, Guid callerId)
        {
            // A user may always read their own record; reading someone else's requires
            // a back-office permission.
            if (callerId != id)
            {
                var access = await _permissions.CheckAsync(callerId, Permissions.BackOfficeRead);
                if (!access.Granted)
                {
                    return access.ToFailure<UserDto>();
                }
            }

            var user = await _users.GetUserByIdAsync(id);
            return user == null
                ? Result<UserDto>.Fail(ResultError.NotFound)
                : Result<UserDto>.Success(user.ToUserDto());
        }

        public async Task<Result<IEnumerable<PermissionDto>>> GetPermissionsAsync(Guid id, Guid callerId)
        {
            // Same rule as GetByIdAsync: self is always allowed (the landing page needs it).
            if (callerId != id)
            {
                var access = await _permissions.CheckAsync(callerId, Permissions.BackOfficeRead);
                if (!access.Granted)
                {
                    return access.ToFailure<IEnumerable<PermissionDto>>();
                }
            }

            var user = await _users.GetUserByIdAsync(id);
            if (user == null)
            {
                return Result<IEnumerable<PermissionDto>>.Fail(ResultError.NotFound);
            }

            var permissions = await _users.GetUserPermissionsAsync(id);
            return Result<IEnumerable<PermissionDto>>.Success(permissions.Select(p => p.ToPermissionDto()));
        }

        public async Task<Result<UserDto>> CreateAsync(Guid callerId, CreateUserRequestDto dto)
        {
            var access = await _permissions.CheckAsync(callerId, Permissions.UserManagement);
            if (!access.Granted)
            {
                return access.ToFailure<UserDto>();
            }

            var user = dto.ToUser();
            await _users.CreateUserAsync(user);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation } pgEx)
            {
                return Result<UserDto>.Fail(ResultError.Conflict, DescribeDuplicate(pgEx.ConstraintName));
            }

            return Result<UserDto>.Success(user.ToUserDto());
        }

        public async Task<Result<UserDto>> UpdateAsync(Guid id, Guid callerId, UpdateUserRequestDto dto)
        {
            var access = await _permissions.CheckAsync(callerId, Permissions.UserManagement);
            if (!access.Granted)
            {
                return access.ToFailure<UserDto>();
            }

            var updated = await _users.UpdateUserAsync(id, dto);
            if (updated == null)
            {
                return Result<UserDto>.Fail(ResultError.NotFound);
            }

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation } pgEx)
            {
                return Result<UserDto>.Fail(ResultError.Conflict, DescribeDuplicate(pgEx.ConstraintName));
            }

            return Result<UserDto>.Success(updated.ToUserDto());
        }

        public async Task<Result<UserDto>> SetActiveAsync(Guid id, Guid callerId, bool isActive)
        {
            var access = await _permissions.CheckAsync(callerId, Permissions.UserManagement);
            if (!access.Granted)
            {
                return access.ToFailure<UserDto>();
            }

            var updated = await _users.SetUserActiveAsync(id, isActive);
            if (updated == null)
            {
                return Result<UserDto>.Fail(ResultError.NotFound);
            }

            await _unitOfWork.SaveChangesAsync();
            return Result<UserDto>.Success(updated.ToUserDto());
        }

        public async Task<Result> DeleteAsync(Guid id, Guid callerId)
        {
            var access = await _permissions.CheckAsync(callerId, Permissions.UserManagement);
            if (!access.Granted)
            {
                return access.ToFailure();
            }

            var user = await _users.GetUserByIdAsync(id);
            if (user == null)
            {
                return Result.Fail(ResultError.NotFound);
            }

            await _users.DeleteUserAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> AddPermissionAsync(Guid id, Guid callerId, Guid permissionId)
        {
            var access = await _permissions.CheckAsync(callerId, Permissions.PermissionManagement);
            if (!access.Granted)
            {
                return access.ToFailure();
            }

            var user = await _users.GetUserByIdAsync(id);
            if (user == null)
            {
                return Result.Fail(ResultError.NotFound);
            }

            var success = await _users.AddPermissionToUserAsync(id, permissionId);
            if (!success)
            {
                return Result.Fail(ResultError.Validation, "Failed to add permission to user.");
            }

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> RemovePermissionAsync(Guid id, Guid callerId, Guid permissionId)
        {
            var access = await _permissions.CheckAsync(callerId, Permissions.PermissionManagement);
            if (!access.Granted)
            {
                return access.ToFailure();
            }

            var user = await _users.GetUserByIdAsync(id);
            if (user == null)
            {
                return Result.Fail(ResultError.NotFound);
            }

            var success = await _users.RemovePermissionFromUserAsync(id, permissionId);
            if (!success)
            {
                return Result.Fail(ResultError.Validation, "Failed to remove permission from user.");
            }

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        // Friendly message for a unique-constraint violation on the User table.
        private static string DescribeDuplicate(string? constraintName) => constraintName switch
        {
            "Kullanıcı_email_key" => "Bu e-posta adresi başka bir kullanıcı tarafından kullanılıyor.",
            "Kullanıcı_telefon_key" => "Bu telefon numarası başka bir kullanıcı tarafından kullanılıyor.",
            "Kullanıcı_kimlikNo_key" => "Bu T.C. kimlik numarası başka bir kullanıcı tarafından kullanılıyor.",
            "Kullanıcı_sicilNo_key" => "Bu sicil numarası başka bir kullanıcı tarafından kullanılıyor.",
            _ => "Bu bilgiler başka bir kullanıcı tarafından kullanılıyor."
        };
    }
}
