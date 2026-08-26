namespace api.Interfaces
{
    public interface IProposalRepository
    {
        Task<IEnumerable<Models.Proposal>> GetAllProposalsAsync();
        Task<Models.Proposal?> GetProposalByIdAsync(Guid id);
        Task<IEnumerable<Models.Proposal>> GetProposalsByUserIdAsync(Guid userId);
        Task<IEnumerable<Models.Evaluation>> GetProposalEvaluationsAsync(Guid proposalId);

        Task<Models.Proposal> CreateProposalAsync(Models.Proposal proposal);
        Task<Models.Proposal?> UpdateProposalAsync(Guid id, Models.Proposal proposal);
        Task<bool> DeleteProposalAsync(Guid id);
    }
}