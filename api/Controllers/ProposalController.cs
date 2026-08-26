using api.Interfaces;
using api.Dtos.Proposal;
using api.Dtos.Evaluation;
using api.Mappers.ProposalMappers;
using api.Mappers.EvaluationMappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/proposals")]
    [ApiController]
    
    public class ProposalController : ControllerBase
    {
        private readonly IProposalRepository _proposalRepository;
        private readonly IEvaluationRepository _evaluationRepository;

        public ProposalController(
            IProposalRepository proposalRepository,
            IEvaluationRepository evaluationRepository)
        {
            _proposalRepository = proposalRepository;
            _evaluationRepository = evaluationRepository;
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

        // GET: api/proposals/{id}/evaluations
        [HttpGet("{id}/evaluations")]
        public async Task<IActionResult> GetProposalEvaluations([FromRoute] Guid id)
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

        // POST: api/proposals/{proposalId}/evaluations
        [HttpPost("{proposalId}/evaluations")]
        public async Task<IActionResult> CreateProposalEvaluation(
            [FromRoute] Guid proposalId,
            [FromBody] CreateEvaluationRequestDto evaluationDto)
        {
            var proposal = await _proposalRepository.GetProposalByIdAsync(proposalId);
            if (proposal == null)
            {
                return NotFound();
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