using api.Authorization;
using api.Common;
using api.Dtos.Evaluation;
using api.Interfaces;
using api.Mappers.EvaluationMappers;

namespace api.Services
{
    public sealed class EvaluationService : IEvaluationService
    {
        private readonly IEvaluationRepository _evaluations;
        private readonly IProposalRepository _proposals;
        private readonly IPermissionChecker _permissions;

        public EvaluationService(
            IEvaluationRepository evaluations,
            IProposalRepository proposals,
            IPermissionChecker permissions)
        {
            _evaluations = evaluations;
            _proposals = proposals;
            _permissions = permissions;
        }

        public async Task<Result<IEnumerable<EvaluationDto>>> GetAllAsync(Guid callerId)
        {
            var access = await _permissions.CheckAsync(callerId, Permissions.EvaluationCreate);
            if (!access.Granted)
            {
                return access.ToFailure<IEnumerable<EvaluationDto>>();
            }

            var evaluations = await _evaluations.GetAllEvaluationsAsync();
            return Result<IEnumerable<EvaluationDto>>.Success(evaluations.Select(evaluation => evaluation.ToEvaluationDto()));
        }

        public async Task<Result<EvaluationDto>> GetByIdAsync(Guid id, Guid callerId)
        {
            var access = await _permissions.CheckAsync(callerId, Permissions.EvaluationCreate);
            if (!access.Granted)
            {
                return access.ToFailure<EvaluationDto>();
            }

            var evaluation = await _evaluations.GetEvaluationByIdAsync(id);
            return evaluation == null
                ? Result<EvaluationDto>.Fail(ResultError.NotFound)
                : Result<EvaluationDto>.Success(evaluation.ToEvaluationDto());
        }

        public async Task<Result<EvaluationDto>> CreateAsync(CreateEvaluationRequestDto dto)
        {
            var access = await _permissions.CheckAsync(dto.UserId, Permissions.EvaluationCreate);
            if (!access.Granted)
            {
                return access.ToFailure<EvaluationDto>();
            }

            var proposal = await _proposals.GetProposalByIdAsync(dto.ProposalId);
            if (proposal == null)
            {
                return Result<EvaluationDto>.Fail(ResultError.Validation, "Proposal does not exist.");
            }

            var created = await _evaluations.CreateEvaluationAsync(dto.ToEvaluation());
            return Result<EvaluationDto>.Success(created.ToEvaluationDto());
        }

        public async Task<Result<EvaluationDto>> UpdateAsync(Guid id, UpdateEvaluationRequestDto dto)
        {
            var access = await _permissions.CheckAsync(dto.UserId, Permissions.EvaluationCreate);
            if (!access.Granted)
            {
                return access.ToFailure<EvaluationDto>();
            }

            var proposal = await _proposals.GetProposalByIdAsync(dto.ProposalId);
            if (proposal == null)
            {
                return Result<EvaluationDto>.Fail(ResultError.Validation, "Proposal does not exist.");
            }

            var updated = await _evaluations.UpdateEvaluationAsync(id, dto.ToEvaluation());
            return updated == null
                ? Result<EvaluationDto>.Fail(ResultError.NotFound)
                : Result<EvaluationDto>.Success(updated.ToEvaluationDto());
        }

        public async Task<Result> DeleteAsync(Guid id, Guid callerId)
        {
            var access = await _permissions.CheckAsync(callerId, Permissions.EvaluationCreate);
            if (!access.Granted)
            {
                return access.ToFailure();
            }

            var deleted = await _evaluations.DeleteEvaluationAsync(id);
            return deleted ? Result.Success() : Result.Fail(ResultError.NotFound);
        }
    }
}
