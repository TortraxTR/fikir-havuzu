using api.Interfaces;
using api.Dtos.Proposal;
using api.Dtos.Evaluation;
using api.Dtos.ProposalFile;
using api.Mappers.ProposalMappers;
using api.Mappers.EvaluationMappers;
using api.Mappers.ProposalFileMappers;
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

        public ProposalController(
            IProposalRepository proposalRepository,
            IEvaluationRepository evaluationRepository,
            IUserRepository userRepository,
            IProposalFileRepository fileRepository)
        {
            _proposalRepository = proposalRepository;
            _evaluationRepository = evaluationRepository;
            _userRepository = userRepository;
            _fileRepository = fileRepository;
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
            if (!user.IsActive || !permissions.Any(permission => permission.Code == "EVALUATION_CREATE"))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Teklifleri listeleme yetkisi gereklidir.");
            }

            var proposals = await _proposalRepository.GetAllProposalsAsync();
            return Ok(proposals.Select(proposal => proposal.ToProposalDto()));
        }

        // GET: api/proposals/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProposalDto>> GetProposal([FromRoute] Guid id)
        {
            var proposal = await _proposalRepository.GetProposalByIdAsync(id);
            if (proposal == null)
            {
                return NotFound();
            }
            return Ok(proposal.ToProposalDto());
        }

        // GET: api/proposals/{id}/evaluations
        [HttpGet("{id}/evaluations")]
        public async Task<ActionResult<IEnumerable<EvaluationDto>>> GetProposalEvaluations([FromRoute] Guid id)
        {
            var proposal = await _proposalRepository.GetProposalByIdAsync(id);
            if (proposal == null)
            {
                return NotFound();
            }

            var evaluations = await _proposalRepository.GetProposalEvaluationsAsync(id);
            var evaluationsDto = evaluations.Select(e => e.ToEvaluationDto());
            return Ok(evaluationsDto);
        }

        // GET: api/proposals/{id}/files
        [HttpGet("{id}/files")]
        public async Task<ActionResult<IEnumerable<ProposalFileDto>>> GetProposalFiles([FromRoute] Guid id)
        {
            var proposal = await _proposalRepository.GetProposalByIdAsync(id);
            if (proposal == null)
            {
                return NotFound();
            }

            var files = await _fileRepository.GetProposalFilesByProposalIdAsync(id);
            return Ok(files.Select(f => f.ToProposalFileDto()));
        }

        // POST: api/proposals/{proposalId}/files
        [HttpPost("{proposalId}/files")]
        public async Task<ActionResult<ProposalFileDto>> CreateProposalFile(
            [FromRoute] Guid proposalId,
            [FromBody] CreateProposalFileRequestDto fileDto)
        {
            var proposal = await _proposalRepository.GetProposalByIdAsync(proposalId);
            if (proposal == null)
            {
                return NotFound();
            }

            var fileEntity = fileDto.ToProposalFile();
            fileEntity.ProposalId = proposalId;

            var createdFile = await _fileRepository.CreateProposalFileAsync(fileEntity);

            return CreatedAtAction(
                nameof(ProposalFileController.GetProposalFileById),
                "ProposalFile",
                new { id = createdFile.Id },
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

            var user = await _userRepository.GetUserByIdAsync(evaluationDto.UserId);
            if (user == null)
            {
                return BadRequest("User does not exist.");
            }

            var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);
            if (!user.IsActive || !permissions.Any(permission => permission.Code == "EVALUATION_CREATE"))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Değerlendirme oluşturma yetkisi gereklidir.");
            }

            var evaluation = evaluationDto.ToEvaluation();
            evaluation.ProposalId = proposalId;

            var createdEvaluation = await _evaluationRepository.CreateEvaluationAsync(evaluation);

            return CreatedAtAction(
                nameof(EvaluationController.GetEvaluation),
                "Evaluation",
                new { id = createdEvaluation.Id },
                createdEvaluation.ToEvaluationDto());
        }

        // POST: api/proposals
        [HttpPost]
        public async Task<ActionResult<ProposalDto>> CreateProposal([FromBody] CreateProposalRequestDto proposalDto)
        {
            var user = await _userRepository.GetUserByIdAsync(proposalDto.UserId);
            if (user == null)
            {
                return BadRequest("User does not exist.");
            }

            var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);
            if (!user.IsActive || !permissions.Any(permission => permission.Code == "PROPOSAL_CREATE"))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Öneri oluşturma yetkisi gereklidir.");
            }

            var createdProposal = await _proposalRepository.CreateProposalAsync(proposalDto.ToProposal());
            return CreatedAtAction(nameof(GetProposal), new { id = createdProposal.Id }, createdProposal.ToProposalDto());
        }

        // PUT: api/proposals/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ProposalDto>> UpdateProposal(Guid id, [FromBody] UpdateProposalRequestDto proposalDto)
        {
            var updatedProposal = await _proposalRepository.UpdateProposalAsync(id, proposalDto.ToProposal());
            if (updatedProposal == null)
            {
                return NotFound();
            }

            return Ok(updatedProposal.ToProposalDto());
        }

        // DELETE: api/proposals/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProposal(Guid id)
        {
            var deleted = await _proposalRepository.DeleteProposalAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }

}