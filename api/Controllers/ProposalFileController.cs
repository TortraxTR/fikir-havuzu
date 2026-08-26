using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProposalFileController : ControllerBase
    {
        private readonly IProposalFileRepository _proposalFileRepository;

        public ProposalFileController(IProposalFileRepository proposalFileRepository)
        {
            _proposalFileRepository = proposalFileRepository;
        }

        // GET: api/ProposalFile
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProposalFile>>> GetAllProposalFiles()
        {
            var proposalFiles = await _proposalFileRepository.GetAllProposalFilesAsync();
            return Ok(proposalFiles);
        }

        // GET: api/ProposalFile/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProposalFile>> GetProposalFileById(Guid id)
        {
            var proposalFile = await _proposalFileRepository.GetProposalFileByIdAsync(id);
            if (proposalFile == null)
            {
                return NotFound();
            }
            return Ok(proposalFile);
        }

        // GET: api/ProposalFile/proposal/{proposalId}
        [HttpGet("proposal/{proposalId}")]
        public async Task<ActionResult<IEnumerable<ProposalFile>>> GetProposalFilesByProposalId(Guid proposalId)
        {
            var proposalFiles = await _proposalFileRepository.GetProposalFilesByProposalIdAsync(proposalId);
            return Ok(proposalFiles);
        }

        // POST: api/ProposalFile
        [HttpPost]
        public async Task<ActionResult<ProposalFile>> CreateProposalFile([FromBody] ProposalFile proposalFile)
        {
            var createdProposalFile = await _proposalFileRepository.CreateProposalFileAsync(proposalFile);
            return CreatedAtAction(nameof(GetProposalFileById), new { id = createdProposalFile.Id }, createdProposalFile);
        }

        // PUT: api/ProposalFile/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ProposalFile>> UpdateProposalFile(Guid id, [FromBody] ProposalFile proposalFile)
        {
            var updatedProposalFile = await _proposalFileRepository.UpdateProposalFileAsync(id, proposalFile);
            if (updatedProposalFile == null)
            {
                return NotFound();
            }
            return Ok(updatedProposalFile);
        }

        // DELETE: api/ProposalFile/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProposalFile(Guid id)
        {
            var deleted = await _proposalFileRepository.DeleteProposalFileAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}