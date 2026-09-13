using api.Models;

namespace api.Interfaces
{
    public interface IProposalFileRepository
    {
        Task<ProposalFile?> GetProposalFileByIdAsync(Guid id);
        Task<IEnumerable<ProposalFile>> GetProposalFilesByProposalIdAsync(Guid proposalId);
        Task<ProposalFile> CreateProposalFileAsync(ProposalFile proposalFile);
    }
}
