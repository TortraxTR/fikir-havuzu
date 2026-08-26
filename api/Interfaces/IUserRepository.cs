namespace api.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<Models.User>> GetAllUsersAsync();
        Task<Models.User?> GetUserByIdAsync(Guid id);
        Task<Models.User> CreateUserAsync(Models.User user);
        Task<Models.User?> UpdateUserAsync(Guid id, Models.User user);
        Task<bool> DeleteUserAsync(Guid id);
    }
}