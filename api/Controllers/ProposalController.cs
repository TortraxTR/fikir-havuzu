using api.Dtos.Proposal;
using api.Mappers.ProposalMappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/proposals")]
    [ApiController]
    
    public class ProposalController : ControllerBase
    {
        private readonly Interfaces.IProposalRepository _proposalRepository;

        public ProposalController(Interfaces.IProposalRepository proposalRepository)
        {
            _proposalRepository = proposalRepository;
        }

        // GET: api/proposals
        [HttpGet]
        public async Task<IActionResult> GetProposals()
        {
            var proposals = await _proposalRepository.GetAllProposalsAsync();
            return Ok(proposals.Select(proposal => proposal.ToProposalDto()));
        }

        // GET: api/proposals/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProposal([FromRoute] Guid id)
        {
            var proposal = await _proposalRepository.GetProposalByIdAsync(id);
            if (proposal == null)
            {
                return NotFound();
            }
            return Ok(proposal.ToProposalDto());
        }

        // POST: api/proposals
        [HttpPost]
        public async Task<IActionResult> CreateProposal([FromBody] CreateProposalRequestDto proposalDto)
        {
            var createdProposal = await _proposalRepository.CreateProposalAsync(proposalDto.ToProposal());
            return CreatedAtAction(nameof(GetProposal), new { id = createdProposal.Id }, createdProposal.ToProposalDto());
        }

        // PUT: api/proposals/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProposal(Guid id, [FromBody] UpdateProposalRequestDto proposalDto)
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
        public async Task<IActionResult> DeleteProposal(Guid id)
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