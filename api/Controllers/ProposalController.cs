using api.Common;
using api.Dtos.Evaluation;
using api.Dtos.Proposal;
using api.Dtos.ProposalFile;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/proposals")]
    [ApiController]
    public class ProposalController : ControllerBase
    {
        private readonly IProposalService _proposals;

        public ProposalController(IProposalService proposals)
        {
            _proposals = proposals;
        }

        // GET: api/proposals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProposalDto>>> GetProposals([FromQuery] Guid userId)
        {
            var result = await _proposals.GetVisibleAsync(userId);
            return result.ToActionResult();
        }

        // GET: api/proposals/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProposalDto>> GetProposal([FromRoute] Guid id, [FromQuery] Guid callerId)
        {
            var result = await _proposals.GetByIdAsync(id, callerId);
            return result.ToActionResult();
        }

        // GET: api/proposals/{id}/evaluations
        [HttpGet("{id}/evaluations")]
        public async Task<ActionResult<IEnumerable<EvaluationDto>>> GetProposalEvaluations([FromRoute] Guid id, [FromQuery] Guid callerId)
        {
            var result = await _proposals.GetEvaluationsAsync(id, callerId);
            return result.ToActionResult();
        }

        // GET: api/proposals/{id}/files
        [HttpGet("{id}/files")]
        public async Task<ActionResult<IEnumerable<ProposalFileDto>>> GetProposalFiles([FromRoute] Guid id, [FromQuery] Guid callerId)
        {
            var result = await _proposals.GetFilesAsync(id, callerId);
            return result.ToActionResult();
        }

        // POST: api/proposals/{proposalId}/files
        [HttpPost("{proposalId}/files")]
        public async Task<ActionResult<ProposalFileDto>> CreateProposalFile(
            [FromRoute] Guid proposalId,
            [FromQuery] Guid callerId,
            [FromBody] CreateProposalFileRequestDto fileDto)
        {
            var result = await _proposals.AddFileAsync(proposalId, callerId, fileDto);
            if (!result.Ok)
            {
                return result.Error();
            }

            return CreatedAtAction(
                nameof(ProposalFileController.GetProposalFileById),
                "ProposalFile",
                new { id = result.Value!.Id, callerId },
                result.Value);
        }

        // POST: api/proposals
        [HttpPost]
        public async Task<ActionResult<ProposalDto>> CreateProposal([FromBody] CreateProposalRequestDto proposalDto)
        {
            var result = await _proposals.CreateAsync(proposalDto);
            if (!result.Ok)
            {
                return result.Error();
            }

            return CreatedAtAction(
                nameof(GetProposal),
                new { id = result.Value!.Id, callerId = proposalDto.UserId },
                result.Value);
        }

        // PUT: api/proposals/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ProposalDto>> UpdateProposal(Guid id, [FromQuery] Guid callerId, [FromBody] UpdateProposalRequestDto proposalDto)
        {
            var result = await _proposals.UpdateAsync(id, callerId, proposalDto);
            return result.ToActionResult();
        }

        // DELETE: api/proposals/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProposal(Guid id, [FromQuery] Guid callerId)
        {
            var result = await _proposals.DeleteAsync(id, callerId);
            return result.ToActionResult();
        }
    }
}
