using api.Common;
using api.Dtos.Evaluation;
using api.Dtos.Proposal;
using api.Dtos.ProposalFile;
using api.Services;
using Microsoft.AspNetCore.Http;
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
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProposalFileDto>> CreateProposalFile(
            [FromRoute] Guid proposalId,
            [FromQuery] Guid callerId,
            IFormFile file)
        {
            var result = await _proposals.AddFileAsync(proposalId, callerId, file);
            if (!result.Ok)
            {
                return result.Error();
            }

            return Created(
                $"/api/proposals/{proposalId}/files/{result.Value!.Id}/content",
                result.Value);
        }

        // GET: api/proposals/{proposalId}/files/{fileId}/content
        [HttpGet("{proposalId}/files/{fileId}/content")]
        public async Task<ActionResult> DownloadProposalFile(
            [FromRoute] Guid proposalId,
            [FromRoute] Guid fileId,
            [FromQuery] Guid callerId)
        {
            var result = await _proposals.DownloadFileAsync(proposalId, fileId, callerId);
            if (!result.Ok)
            {
                return result.Error();
            }

            var file = result.Value!;
            return File(file.Content, file.ContentType, file.FileName);
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
