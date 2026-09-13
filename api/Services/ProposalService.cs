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
using Microsoft.AspNetCore.Http;

namespace api.Services
{
    public sealed class ProposalService : IProposalService
    {
        private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

        private readonly IProposalRepository _proposals;
        private readonly IProposalFileRepository _files;
        private readonly IPermissionChecker _permissions;
        private readonly IUnitOfWork _unitOfWork;

        public ProposalService(
            IProposalRepository proposals,
            IProposalFileRepository files,
            IPermissionChecker permissions,
            IUnitOfWork unitOfWork)
        {
            _proposals = proposals;
            _files = files;
            _permissions = permissions;
            _unitOfWork = unitOfWork;
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

        public async Task<Result<ProposalFileDto>> AddFileAsync(Guid proposalId, Guid callerId, IFormFile file)
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

            if (file.Length == 0)
            {
                return Result<ProposalFileDto>.Fail(ResultError.Validation, "Dosya boş olamaz.");
            }

            if (file.Length > MaxFileSizeBytes)
            {
                return Result<ProposalFileDto>.Fail(ResultError.Validation, "Dosya boyutu 10 MB'ı geçemez.");
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            var entity = new ProposalFile
            {
                ProposalId = proposalId,
                FileName = file.FileName,
                ContentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
                Content = stream.ToArray()
            };

            var created = await _files.CreateProposalFileAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result<ProposalFileDto>.Success(created.ToProposalFileDto());
        }

        public async Task<Result<ProposalFileContent>> DownloadFileAsync(Guid proposalId, Guid fileId, Guid callerId)
        {
            var proposal = await _proposals.GetProposalByIdAsync(proposalId);
            if (proposal == null)
            {
                return Result<ProposalFileContent>.Fail(ResultError.NotFound);
            }

            var access = await AuthorizeViewAsync(callerId, proposal);
            if (!access.Ok)
            {
                return Result<ProposalFileContent>.Fail(access);
            }

            var file = await _files.GetProposalFileByIdAsync(fileId);
            if (file == null || file.ProposalId != proposalId)
            {
                return Result<ProposalFileContent>.Fail(ResultError.NotFound);
            }

            return Result<ProposalFileContent>.Success(new ProposalFileContent(file.FileName, file.ContentType, file.Content));
        }

        public async Task<Result<ProposalDto>> CreateAsync(CreateProposalRequestDto dto)
        {
            var access = await _permissions.CheckAsync(dto.UserId, Permissions.ProposalCreate);
            if (!access.Granted)
            {
                return access.ToFailure<ProposalDto>();
            }

            var created = await _proposals.CreateProposalAsync(dto.ToProposal());
            await _unitOfWork.SaveChangesAsync();

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
            if (updated == null)
            {
                return Result<ProposalDto>.Fail(ResultError.NotFound);
            }

            await _unitOfWork.SaveChangesAsync();
            return Result<ProposalDto>.Success(updated.ToProposalDto());
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
            if (!deleted)
            {
                return Result.Fail(ResultError.NotFound);
            }

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
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
