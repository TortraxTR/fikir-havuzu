using api.Interfaces;

namespace api.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly FikirHavuzuContext _context;

        public UnitOfWork(FikirHavuzuContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
