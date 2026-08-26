using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly FikirHavuzuContext _context;

        public UserRepository(FikirHavuzuContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateUserAsync(Guid id, User user)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.Name = user.Name;
            existingUser.Surname = user.Surname;
            existingUser.Phone = user.Phone;
            existingUser.RegistrationNo = user.RegistrationNo;
            existingUser.GovernmentId = user.GovernmentId;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.IsActive = user.IsActive;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.Permissions ?? Enumerable.Empty<Permission>();
        }

        public async Task<bool> AddPermissionToUserAsync(Guid userId, Guid permissionId)
        {
            var user = await _context.Users
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var permission = await _context.Permissions.FindAsync(permissionId);

            if (user == null || permission == null)
            {
                return false;
            }

            user.Permissions.Add(permission);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemovePermissionFromUserAsync(Guid userId, Guid permissionId)
        {
            var user = await _context.Users
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var permission = await _context.Permissions.FindAsync(permissionId);

            if (user == null || permission == null)
            {
                return false;
            }

            user.Permissions.Remove(permission);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}