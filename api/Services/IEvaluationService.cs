using api.Common;
using api.Dtos.Evaluation;

namespace api.Services
{
    public interface IEvaluationService
    {
        Task<Result<IEnumerable<EvaluationDto>>> GetAllAsync(Guid callerId);

        Task<Result<EvaluationDto>> GetByIdAsync(Guid id, Guid callerId);

        Task<Result<EvaluationDto>> CreateAsync(CreateEvaluationRequestDto dto);

        Task<Result<EvaluationDto>> UpdateAsync(Guid id, UpdateEvaluationRequestDto dto);

        Task<Result> DeleteAsync(Guid id, Guid callerId);
    }
}
