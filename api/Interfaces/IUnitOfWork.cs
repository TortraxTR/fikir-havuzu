namespace api.Interfaces
{
    // The transaction boundary. Repositories stage changes (Add/Update/Remove) without
    // committing; a service calls SaveChangesAsync once it has staged everything a single
    // request should persist together.
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
