namespace api.Interfaces
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Models.Permission>> GetAllPermissionsAsync();
        Task<Models.Permission?> GetPermissionByIdAsync(Guid id);
        Task<Models.Permission> CreatePermissionAsync(Models.Permission permission);
        Task<Models.Permission?> UpdatePermissionAsync(Guid id, Models.Permission permission);
        Task<bool> DeletePermissionAsync(Guid id);
    }
}