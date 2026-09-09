using api.Authorization;
using api.Interfaces;
using api.Dtos.Proposal;
using api.Dtos.Evaluation;
using api.Dtos.ProposalFile;
using api.Mappers.ProposalMappers;
using api.Mappers.EvaluationMappers;
using api.Mappers.ProposalFileMappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/proposals")]
    [ApiController]

    public class ProposalController : ControllerBase
    {
        private readonly IProposalRepository _proposalRepository;
        private readonly IEvaluationRepository _evaluationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProposalFileRepository _fileRepository;
        private readonly IPermissionGuard _guard;

        public ProposalController(
            IProposalRepository proposalRepository,
            IEvaluationRepository evaluationRepository,
            IUserRepository userRepository,
            IProposalFileRepository fileRepository,
            IPermissionGuard guard)
        {
            _proposalRepository = proposalRepository;
            _evaluationRepository = evaluationRepository;
            _userRepository = userRepository;
            _fileRepository = fileRepository;
            _guard = guard;
        }

        // GET: api/proposals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProposalDto>>> GetProposals([FromQuery] Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized("Geçerli bir kullanıcı gereklidir.");
            }

            var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);
            if (!user.IsActive)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Aktif bir kullanıcı gereklidir.");
            }

            var canViewAllProposals = permissions.Any(permission => permission.Code == Permissions.EvaluationCreate);
            var proposals = canViewAllProposals
                ? await _proposalRepository.GetAllProposalsAsync()
                : await _proposalRepository.GetProposalsByUserIdAsync(user.Id);

            return Ok(proposals.Select(proposal => proposal.ToProposalDto()));
        }

        // GET: api/proposals/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProposalDto>> GetProposal([FromRoute] Guid id, [FromQuery] Guid callerId)
        {
            var proposal = await _proposalRepository.GetProposalByIdAsync(id);
            if (proposal == null)
            {
                return NotFound();
            }

            var access = await AuthorizeProposalViewAsync(callerId, proposal);
            if (access != null)
            {
                return access;
            }

            return Ok(proposal.ToProposalDto());
        }

        // GET: api/proposals/{id}/evaluations
        [HttpGet("{id}/evaluations")]
        public async Task<ActionResult<IEnumerable<EvaluationDto>>> GetProposalEvaluations([FromRoute] Guid id, [FromQuery] Guid callerId)
        {
            var proposal = await _proposalRepository.GetProposalByIdAsync(id);
            if (proposal == null)
            {
                return NotFound();
            }

            var access = await AuthorizeProposalViewAsync(callerId, proposal);
            if (access != null)
            {
                return access;
            }

            var evaluations = await _proposalRepository.GetProposalEvaluationsAsync(id);
            var evaluationsDto = evaluations.Select(e => e.ToEvaluationDto());
            return Ok(evaluationsDto);
        }

        // GET: api/proposals/{id}/files
        [HttpGet("{id}/files")]
        public async Task<ActionResult<IEnumerable<ProposalFileDto>>> GetProposalFiles([FromRoute] Guid id, [FromQuery] Guid callerId)
        {
            var proposal = await _proposalRepository.GetProposalByIdAsync(id);
            if (proposal == null)
            {
                return NotFound();
            }

            var access = await AuthorizeProposalViewAsync(callerId, proposal);
            if (access != null)
            {
                return access;
            }

            var files = await _fileRepository.GetProposalFilesByProposalIdAsync(id);
            return Ok(files.Select(f => f.ToProposalFileDto()));
        }

        // POST: api/proposals/{proposalId}/files
        [HttpPost("{proposalId}/files")]
        public async Task<ActionResult<ProposalFileDto>> CreateProposalFile(
            [FromRoute] Guid proposalId,
            [FromQuery] Guid callerId,
            [FromBody] CreateProposalFileRequestDto fileDto)
        {
            var proposal = await _proposalRepository.GetProposalByIdAsync(proposalId);
            if (proposal == null)
            {
                return NotFound();
            }

            // Only the proposal owner attaches documents to their own idea (spec 2.1).
            var authorization = await _guard.RequireAsync(callerId, Permissions.ProposalCreate);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            if (proposal.UserId != callerId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Yalnızca fikri oluşturan kullanıcı doküman ekleyebilir.");
            }

            var fileEntity = fileDto.ToProposalFile();
            fileEntity.ProposalId = proposalId;

            var createdFile = await _fileRepository.CreateProposalFileAsync(fileEntity);

            return CreatedAtAction(
                nameof(ProposalFileController.GetProposalFileById),
                "ProposalFile",
                new { id = createdFile.Id, callerId },
                createdFile.ToProposalFileDto()
            );
        }

        // POST: api/proposals/{proposalId}/evaluations
        [HttpPost("{proposalId}/evaluations")]
        public async Task<ActionResult<EvaluationDto>> CreateProposalEvaluation(
            [FromRoute] Guid proposalId,
            [FromBody] CreateEvaluationRequestDto evaluationDto)
        {
            var proposal = await _proposalRepository.GetProposalByIdAsync(proposalId);
            if (proposal == null)
            {
                return NotFound();
            }

            var authorization = await _guard.RequireAsync(evaluationDto.UserId, Permissions.EvaluationCreate);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var evaluation = evaluationDto.ToEvaluation();
            evaluation.ProposalId = proposalId;

            var createdEvaluation = await _evaluationRepository.CreateEvaluationAsync(evaluation);

            return CreatedAtAction(
                nameof(EvaluationController.GetEvaluation),
                "Evaluation",
                new { id = createdEvaluation.Id, callerId = evaluationDto.UserId },
                createdEvaluation.ToEvaluationDto());
        }

        // POST: api/proposals
        [HttpPost]
        public async Task<ActionResult<ProposalDto>> CreateProposal([FromBody] CreateProposalRequestDto proposalDto)
        {
            var authorization = await _guard.RequireAsync(proposalDto.UserId, Permissions.ProposalCreate);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var createdProposal = await _proposalRepository.CreateProposalAsync(proposalDto.ToProposal());
            return CreatedAtAction(nameof(GetProposal), new { id = createdProposal.Id, callerId = proposalDto.UserId }, createdProposal.ToProposalDto());
        }

        // PUT: api/proposals/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ProposalDto>> UpdateProposal(Guid id, [FromQuery] Guid callerId, [FromBody] UpdateProposalRequestDto proposalDto)
        {
            var proposal = await _proposalRepository.GetProposalByIdAsync(id);
            if (proposal == null)
            {
                return NotFound();
            }

            var authorization = await _guard.RequireAsync(callerId, Permissions.ProposalCreate);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            if (proposal.UserId != callerId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Yalnızca fikri oluşturan kullanıcı düzenleyebilir.");
            }

            var updatedProposal = await _proposalRepository.UpdateProposalAsync(id, proposalDto.ToProposal());
            if (updatedProposal == null)
            {
                return NotFound();
            }

            return Ok(updatedProposal.ToProposalDto());
        }

        // DELETE: api/proposals/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProposal(Guid id, [FromQuery] Guid callerId)
        {
            var proposal = await _proposalRepository.GetProposalByIdAsync(id);
            if (proposal == null)
            {
                return NotFound();
            }

            var isOwner = proposal.UserId == callerId;
            var authorization = await _guard.RequireAsync(callerId, isOwner ? Array.Empty<string>() : new[] { Permissions.UserManagement });
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var deleted = await _proposalRepository.DeleteProposalAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// A proposal (and its evaluations and files) may be viewed by its owner or by
        /// any active user who can evaluate proposals. Returns <c>null</c> when allowed,
        /// otherwise the error result to return.
        /// </summary>
        private async Task<ActionResult?> AuthorizeProposalViewAsync(Guid callerId, Proposal proposal)
        {
            if (proposal.UserId == callerId)
            {
                var ownerCheck = await _guard.RequireAsync(callerId);
                return ownerCheck.Ok ? null : ownerCheck.ToActionResult();
            }

            var authorization = await _guard.RequireAsync(callerId, Permissions.EvaluationCreate);
            return authorization.Ok ? null : authorization.ToActionResult();
        }
    }

}
