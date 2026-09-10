using api.Authorization;
using api.Common;
using api.Dtos.Evaluation;
using api.Dtos.Proposal;
using api.Dtos.ProposalFile;
using api.Interfaces;
using api.Mappers.EvaluationMappers;
using api.Mappers.ProposalMappers;
using api.Mappers.ProposalFileMappers;
using api.Models;

namespace api.Services
{
    public sealed class ProposalService : IProposalService
    {
        private readonly IProposalRepository _proposals;
        private readonly IProposalFileRepository _files;
        private readonly IPermissionChecker _permissions;

        public ProposalService(
            IProposalRepository proposals,
            IProposalFileRepository files,
            IPermissionChecker permissions)
        {
            _proposals = proposals;
            _files = files;
            _permissions = permissions;
        }

        public async Task<Result<IEnumerable<ProposalDto>>> GetVisibleAsync(Guid callerId)
        {
            var caller = await _permissions.CheckAsync(callerId);
            if (!caller.Granted)
            {
                return caller.ToFailure<IEnumerable<ProposalDto>>();
            }

            var canViewAll = (await _permissions.CheckAsync(callerId, Permissions.EvaluationCreate)).Granted;
            var proposals = canViewAll
                ? await _proposals.GetAllProposalsAsync()
                : await _proposals.GetProposalsByUserIdAsync(callerId);

            return Result<IEnumerable<ProposalDto>>.Success(proposals.Select(proposal => proposal.ToProposalDto()));
        }

        public async Task<Result<ProposalDto>> GetByIdAsync(Guid id, Guid callerId)
        {
            var proposal = await _proposals.GetProposalByIdAsync(id);
            if (proposal == null)
            {
                return Result<ProposalDto>.Fail(ResultError.NotFound);
            }

            var access = await AuthorizeViewAsync(callerId, proposal);
            return access.Ok
                ? Result<ProposalDto>.Success(proposal.ToProposalDto())
                : Result<ProposalDto>.Fail(access);
        }

        public async Task<Result<IEnumerable<EvaluationDto>>> GetEvaluationsAsync(Guid proposalId, Guid callerId)
        {
            var proposal = await _proposals.GetProposalByIdAsync(proposalId);
            if (proposal == null)
            {
                return Result<IEnumerable<EvaluationDto>>.Fail(ResultError.NotFound);
            }

            var access = await AuthorizeViewAsync(callerId, proposal);
            if (!access.Ok)
            {
                return Result<IEnumerable<EvaluationDto>>.Fail(access);
            }

            var evaluations = await _proposals.GetProposalEvaluationsAsync(proposalId);
            return Result<IEnumerable<EvaluationDto>>.Success(evaluations.Select(evaluation => evaluation.ToEvaluationDto()));
        }

        public async Task<Result<IEnumerable<ProposalFileDto>>> GetFilesAsync(Guid proposalId, Guid callerId)
        {
            var proposal = await _proposals.GetProposalByIdAsync(proposalId);
            if (proposal == null)
            {
                return Result<IEnumerable<ProposalFileDto>>.Fail(ResultError.NotFound);
            }

            var access = await AuthorizeViewAsync(callerId, proposal);
            if (!access.Ok)
            {
                return Result<IEnumerable<ProposalFileDto>>.Fail(access);
            }

            var files = await _files.GetProposalFilesByProposalIdAsync(proposalId);
            return Result<IEnumerable<ProposalFileDto>>.Success(files.Select(file => file.ToProposalFileDto()));
        }

        public async Task<Result<ProposalFileDto>> AddFileAsync(Guid proposalId, Guid callerId, CreateProposalFileRequestDto dto)
        {
            var proposal = await _proposals.GetProposalByIdAsync(proposalId);
            if (proposal == null)
            {
                return Result<ProposalFileDto>.Fail(ResultError.NotFound);
            }

            // Only the proposal owner attaches documents to their own idea (spec 2.1).
            var access = await _permissions.CheckAsync(callerId, Permissions.ProposalCreate);
            if (!access.Granted)
            {
                return access.ToFailure<ProposalFileDto>();
            }

            if (proposal.UserId != callerId)
            {
                return Result<ProposalFileDto>.Fail(ResultError.Forbidden, "Yalnızca fikri oluşturan kullanıcı doküman ekleyebilir.");
            }

            var entity = dto.ToProposalFile();
            entity.ProposalId = proposalId;

            var created = await _files.CreateProposalFileAsync(entity);
            return Result<ProposalFileDto>.Success(created.ToProposalFileDto());
        }

        public async Task<Result<ProposalDto>> CreateAsync(CreateProposalRequestDto dto)
        {
            var access = await _permissions.CheckAsync(dto.UserId, Permissions.ProposalCreate);
            if (!access.Granted)
            {
                return access.ToFailure<ProposalDto>();
            }

            var created = await _proposals.CreateProposalAsync(dto.ToProposal());

            // Re-read so the owner navigation is populated for the DTO projection.
            var full = await _proposals.GetProposalByIdAsync(created.Id) ?? created;
            return Result<ProposalDto>.Success(full.ToProposalDto());
        }

        public async Task<Result<ProposalDto>> UpdateAsync(Guid id, Guid callerId, UpdateProposalRequestDto dto)
        {
            var proposal = await _proposals.GetProposalByIdAsync(id);
            if (proposal == null)
            {
                return Result<ProposalDto>.Fail(ResultError.NotFound);
            }

            var access = await _permissions.CheckAsync(callerId, Permissions.ProposalCreate);
            if (!access.Granted)
            {
                return access.ToFailure<ProposalDto>();
            }

            if (proposal.UserId != callerId)
            {
                return Result<ProposalDto>.Fail(ResultError.Forbidden, "Yalnızca fikri oluşturan kullanıcı düzenleyebilir.");
            }

            var updated = await _proposals.UpdateProposalAsync(id, dto.ToProposal());
            return updated == null
                ? Result<ProposalDto>.Fail(ResultError.NotFound)
                : Result<ProposalDto>.Success(updated.ToProposalDto());
        }

        public async Task<Result> DeleteAsync(Guid id, Guid callerId)
        {
            var proposal = await _proposals.GetProposalByIdAsync(id);
            if (proposal == null)
            {
                return Result.Fail(ResultError.NotFound);
            }

            var isOwner = proposal.UserId == callerId;
            var access = isOwner
                ? await _permissions.CheckAsync(callerId)
                : await _permissions.CheckAsync(callerId, Permissions.UserManagement);
            if (!access.Granted)
            {
                return access.ToFailure();
            }

            var deleted = await _proposals.DeleteProposalAsync(id);
            return deleted ? Result.Success() : Result.Fail(ResultError.NotFound);
        }

        // Owner, or any active user who can evaluate proposals.
        private async Task<Result> AuthorizeViewAsync(Guid callerId, Proposal proposal)
        {
            var access = proposal.UserId == callerId
                ? await _permissions.CheckAsync(callerId)
                : await _permissions.CheckAsync(callerId, Permissions.EvaluationCreate);

            return access.Granted ? Result.Success() : access.ToFailure();
        }
    }
}
