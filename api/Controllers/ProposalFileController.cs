using api.Authorization;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using api.Models;

namespace api.Controllers
{
    /// <summary>
    /// Low-level maintenance access to proposal file records. The user-facing flow
    /// lives on <see cref="ProposalController"/>; these endpoints are restricted to
    /// user-management operators.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProposalFileController : ControllerBase
    {
        private readonly IProposalFileRepository _proposalFileRepository;
        private readonly IPermissionGuard _guard;

        public ProposalFileController(IProposalFileRepository proposalFileRepository, IPermissionGuard guard)
        {
            _proposalFileRepository = proposalFileRepository;
            _guard = guard;
        }

        // GET: api/ProposalFile
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProposalFile>>> GetAllProposalFiles([FromQuery] Guid callerId)
        {
            var authorization = await _guard.RequireAsync(callerId, Permissions.UserManagement);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var proposalFiles = await _proposalFileRepository.GetAllProposalFilesAsync();
            return Ok(proposalFiles);
        }

        // GET: api/ProposalFile/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProposalFile>> GetProposalFileById(Guid id, [FromQuery] Guid callerId)
        {
            var authorization = await _guard.RequireAsync(callerId, Permissions.UserManagement);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var proposalFile = await _proposalFileRepository.GetProposalFileByIdAsync(id);
            if (proposalFile == null)
            {
                return NotFound();
            }
            return Ok(proposalFile);
        }

        // GET: api/ProposalFile/proposal/{proposalId}
        [HttpGet("proposal/{proposalId}")]
        public async Task<ActionResult<IEnumerable<ProposalFile>>> GetProposalFilesByProposalId(Guid proposalId, [FromQuery] Guid callerId)
        {
            var authorization = await _guard.RequireAsync(callerId, Permissions.UserManagement);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var proposalFiles = await _proposalFileRepository.GetProposalFilesByProposalIdAsync(proposalId);
            return Ok(proposalFiles);
        }

        // POST: api/ProposalFile
        [HttpPost]
        public async Task<ActionResult<ProposalFile>> CreateProposalFile([FromQuery] Guid callerId, [FromBody] ProposalFile proposalFile)
        {
            var authorization = await _guard.RequireAsync(callerId, Permissions.UserManagement);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var createdProposalFile = await _proposalFileRepository.CreateProposalFileAsync(proposalFile);
            return CreatedAtAction(nameof(GetProposalFileById), new { id = createdProposalFile.Id, callerId }, createdProposalFile);
        }

        // PUT: api/ProposalFile/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ProposalFile>> UpdateProposalFile(Guid id, [FromQuery] Guid callerId, [FromBody] ProposalFile proposalFile)
        {
            var authorization = await _guard.RequireAsync(callerId, Permissions.UserManagement);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var updatedProposalFile = await _proposalFileRepository.UpdateProposalFileAsync(id, proposalFile);
            if (updatedProposalFile == null)
            {
                return NotFound();
            }
            return Ok(updatedProposalFile);
        }

        // DELETE: api/ProposalFile/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProposalFile(Guid id, [FromQuery] Guid callerId)
        {
            var authorization = await _guard.RequireAsync(callerId, Permissions.UserManagement);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var deleted = await _proposalFileRepository.DeleteProposalFileAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
