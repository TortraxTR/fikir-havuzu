using api.Models;

namespace api.Interfaces
{
    public interface IProposalFileRepository
    {
        Task<IEnumerable<ProposalFile>> GetAllProposalFilesAsync();
        Task<ProposalFile?> GetProposalFileByIdAsync(Guid id);
        Task<IEnumerable<ProposalFile>> GetProposalFilesByProposalIdAsync(Guid proposalId);
        Task<ProposalFile> CreateProposalFileAsync(ProposalFile proposalFile);
        Task<ProposalFile?> UpdateProposalFileAsync(Guid id, ProposalFile proposalFile);
        Task<bool> DeleteProposalFileAsync(Guid id);
    }
}
