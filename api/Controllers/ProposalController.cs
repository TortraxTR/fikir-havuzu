using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return Ok(proposals);
        }

        // GET: api/proposals/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProposal([FromRoute] int id)
        {
            var proposal = await _proposalRepository.GetProposalByIdAsync(id);
            if (proposal == null)
            {
                return NotFound();
            }
            return Ok(proposal);
        }

        // POST: api/proposals
        [HttpPost("{creator_id}")]
        public async Task<IActionResult> CreateProposal([FromBody] Models.Proposal proposal)
        {
            var createdProposal = await _proposalRepository.CreateProposalAsync(proposal);
            return CreatedAtAction(nameof(GetProposal), new { id = createdProposal.Id }, createdProposal);
        }


    }

}