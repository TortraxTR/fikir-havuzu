using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class PermissionRepository: IPermissionRepository
    {
        private readonly FikirHavuzuContext _context;
        public PermissionRepository(FikirHavuzuContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            return await _context.Permissions.ToListAsync();
        }

        public async Task<Permission?> GetPermissionByIdAsync(Guid id)
        {
            return await _context.Permissions.FindAsync(id);
        }

        public async Task<Permission> CreatePermissionAsync(Permission permission)
        {
            await _context.Permissions.AddAsync(permission);
            await _context.SaveChangesAsync();
            return permission;
        }

        public async Task<Permission?> UpdatePermissionAsync(Guid id, Permission permission)
        {
            var existingPermission = await _context.Permissions.FindAsync(id);
            if (existingPermission == null)
            {
                return null;
            }

            existingPermission.Name = permission.Name;

            await _context.SaveChangesAsync();
            return existingPermission;
        }

        public async Task<bool> DeletePermissionAsync(Guid id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null)
            {
                return false;
            }

            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}