using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<Models.User>> GetAllUsersAsync();
        Task<Models.User?> GetUserByIdAsync(int id);
        Task<Models.User> CreateUserAsync(Models.User user);
        Task<Models.User?> UpdateUserAsync(int id, Models.User user);
        Task<bool> DeleteUserAsync(int id);
    }
}