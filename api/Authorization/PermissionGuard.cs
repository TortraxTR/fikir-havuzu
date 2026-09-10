using Microsoft.AspNetCore.Mvc;

namespace api.Authorization
{
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
        Task<AuthorizationResult> RequireAsync(Guid callerId, params string[] anyOf);
    }

    // Controller-facing adapter over IPermissionChecker; new service-layer code should
    // depend on IPermissionChecker directly.
    public sealed class PermissionGuard : IPermissionGuard
    {
        private readonly IPermissionChecker _checker;

        public PermissionGuard(IPermissionChecker checker)
        {
            _checker = checker;
        }

        public async Task<AuthorizationResult> RequireAsync(Guid callerId, params string[] anyOf)
        {
            var access = await _checker.CheckAsync(callerId, anyOf);

            return access.Status switch
            {
                AccessStatus.Granted => AuthorizationResult.Success,
                AccessStatus.NoUser => AuthorizationResult.Fail(
                    StatusCodes.Status401Unauthorized, "Geçerli bir kullanıcı gereklidir."),
                AccessStatus.Inactive => AuthorizationResult.Fail(
                    StatusCodes.Status403Forbidden, "Aktif bir kullanıcı gereklidir."),
                _ => AuthorizationResult.Fail(
                    StatusCodes.Status403Forbidden, "Bu işlem için yetkiniz yok."),
            };
        }
    }
}
