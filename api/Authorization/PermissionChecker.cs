using api.Interfaces;

namespace api.Authorization
{
    public sealed class PermissionChecker : IPermissionChecker
    {
        private readonly IUserRepository _users;

        public PermissionChecker(IUserRepository users)
        {
            _users = users;
        }

        public async Task<AccessResult> CheckAsync(Guid callerId, params string[] anyOf)
        {
            if (callerId == Guid.Empty)
            {
                return new AccessResult(AccessStatus.NoUser);
            }

            var caller = await _users.GetUserByIdAsync(callerId);
            if (caller == null)
            {
                return new AccessResult(AccessStatus.NoUser);
            }

            if (!caller.IsActive)
            {
                return new AccessResult(AccessStatus.Inactive);
            }

            if (anyOf.Length == 0)
            {
                return AccessResult.Ok;
            }

            var permissions = await _users.GetUserPermissionsAsync(callerId);
            var held = permissions.Select(permission => permission.Code).ToHashSet();

            return anyOf.Any(held.Contains)
                ? AccessResult.Ok
                : new AccessResult(AccessStatus.Denied);
        }
    }
}
