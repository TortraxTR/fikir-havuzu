using api.Common;
using api.Dtos.Evaluation;
using api.Dtos.Proposal;
using api.Dtos.ProposalFile;

namespace api.Services
{
    public interface IProposalService
    {
        Task<Result<IEnumerable<ProposalDto>>> GetVisibleAsync(Guid callerId);

        Task<Result<ProposalDto>> GetByIdAsync(Guid id, Guid callerId);

        Task<Result<IEnumerable<EvaluationDto>>> GetEvaluationsAsync(Guid proposalId, Guid callerId);

        Task<Result<IEnumerable<ProposalFileDto>>> GetFilesAsync(Guid proposalId, Guid callerId);

        Task<Result<ProposalFileDto>> AddFileAsync(Guid proposalId, Guid callerId, CreateProposalFileRequestDto dto);

        Task<Result<ProposalDto>> CreateAsync(CreateProposalRequestDto dto);

        Task<Result<ProposalDto>> UpdateAsync(Guid id, Guid callerId, UpdateProposalRequestDto dto);

        Task<Result> DeleteAsync(Guid id, Guid callerId);
    }
}
