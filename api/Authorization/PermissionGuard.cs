using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Authorization
{
    /// <summary>
    /// Outcome of an authorization check. When <see cref="Ok"/> is <c>false</c> the
    /// caller returns <see cref="ToActionResult"/> immediately.
    /// </summary>
    public sealed class AuthorizationResult
    {
        public bool Ok { get; private init; }
        public int StatusCode { get; private init; }
        public string Message { get; private init; } = string.Empty;

        public static AuthorizationResult Success { get; } = new() { Ok = true };

        public static AuthorizationResult Fail(int statusCode, string message) =>
            new() { Ok = false, StatusCode = statusCode, Message = message };

        public ActionResult ToActionResult() => new ObjectResult(Message) { StatusCode = StatusCode };
    }

    public interface IPermissionGuard
    {
        /// <summary>
        /// Verifies that <paramref name="callerId"/> is an existing, active user who
        /// holds at least one of <paramref name="anyOf"/>. Pass no codes to require
        /// only a valid, active user.
        /// </summary>
        Task<AuthorizationResult> RequireAsync(Guid callerId, params string[] anyOf);
    }

    public sealed class PermissionGuard : IPermissionGuard
    {
        private readonly IUserRepository _users;

        public PermissionGuard(IUserRepository users)
        {
            _users = users;
        }

        public async Task<AuthorizationResult> RequireAsync(Guid callerId, params string[] anyOf)
        {
            if (callerId == Guid.Empty)
            {
                return AuthorizationResult.Fail(StatusCodes.Status401Unauthorized, "Geçerli bir kullanıcı gereklidir.");
            }

            var caller = await _users.GetUserByIdAsync(callerId);
            if (caller == null)
            {
                return AuthorizationResult.Fail(StatusCodes.Status401Unauthorized, "Geçerli bir kullanıcı gereklidir.");
            }

            if (!caller.IsActive)
            {
                return AuthorizationResult.Fail(StatusCodes.Status403Forbidden, "Aktif bir kullanıcı gereklidir.");
            }

            if (anyOf.Length == 0)
            {
                return AuthorizationResult.Success;
            }

            var permissions = await _users.GetUserPermissionsAsync(callerId);
            var held = permissions.Select(permission => permission.Code).ToHashSet();

            return anyOf.Any(held.Contains)
                ? AuthorizationResult.Success
                : AuthorizationResult.Fail(StatusCodes.Status403Forbidden, "Bu işlem için yetkiniz yok.");
        }
    }
}
