namespace api.Authorization
{
    public enum AccessStatus
    {
        Granted = 0,
        NoUser,
        Inactive,
        Denied
    }

    public readonly record struct AccessResult(AccessStatus Status)
    {
        public bool Granted => Status == AccessStatus.Granted;

        public static AccessResult Ok { get; } = new(AccessStatus.Granted);
    }

    public interface IPermissionChecker
    {
        Task<AccessResult> CheckAsync(Guid callerId, params string[] anyOf);
    }
}
