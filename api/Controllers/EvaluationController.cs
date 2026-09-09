using api.Authorization;
using api.Interfaces;
using api.Models;
using api.Dtos.Evaluation;
using api.Mappers.EvaluationMappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/evaluations")]
    [ApiController]
    public class EvaluationController : ControllerBase
    {
        private readonly IEvaluationRepository _evaluationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProposalRepository _proposalRepository;
        private readonly IPermissionGuard _guard;

        public EvaluationController(
            IEvaluationRepository evaluationRepository,
            IUserRepository userRepository,
            IProposalRepository proposalRepository,
            IPermissionGuard guard)
        {
            _evaluationRepository = evaluationRepository;
            _userRepository = userRepository;
            _proposalRepository = proposalRepository;
            _guard = guard;
        }

        // GET: api/evaluations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EvaluationDto>>> GetEvaluations([FromQuery] Guid callerId)
        {
            var authorization = await _guard.RequireAsync(callerId, Permissions.EvaluationCreate);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var evaluations = await _evaluationRepository.GetAllEvaluationsAsync();
            return Ok(evaluations.Select(evaluation => evaluation.ToEvaluationDto()));
        }

        // GET: api/evaluations/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EvaluationDto>> GetEvaluation([FromRoute] Guid id, [FromQuery] Guid callerId)
        {
            var authorization = await _guard.RequireAsync(callerId, Permissions.EvaluationCreate);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var evaluation = await _evaluationRepository.GetEvaluationByIdAsync(id);
            if (evaluation == null)
            {
                return NotFound();
            }
            return Ok(evaluation.ToEvaluationDto());
        }

        // POST: api/evaluations
        [HttpPost]
        public async Task<ActionResult<EvaluationDto>> CreateEvaluation([FromBody] CreateEvaluationRequestDto evaluationDto)
        {
            var authorization = await _guard.RequireAsync(evaluationDto.UserId, Permissions.EvaluationCreate);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var proposal = await _proposalRepository.GetProposalByIdAsync(evaluationDto.ProposalId);
            if (proposal == null)
            {
                return BadRequest("Proposal does not exist.");
            }

            var evaluation = evaluationDto.ToEvaluation();
            var createdEvaluation = await _evaluationRepository.CreateEvaluationAsync(evaluation);
            return CreatedAtAction(nameof(GetEvaluation), new { id = createdEvaluation.Id, callerId = evaluationDto.UserId }, createdEvaluation.ToEvaluationDto());
        }

        // PUT: api/evaluations/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<EvaluationDto>> UpdateEvaluation([FromRoute] Guid id, [FromBody] UpdateEvaluationRequestDto evaluationDto)
        {
            var authorization = await _guard.RequireAsync(evaluationDto.UserId, Permissions.EvaluationCreate);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var proposal = await _proposalRepository.GetProposalByIdAsync(evaluationDto.ProposalId);
            if (proposal == null)
            {
                return BadRequest("Proposal does not exist.");
            }

            var evaluation = evaluationDto.ToEvaluation();
            var updatedEvaluation = await _evaluationRepository.UpdateEvaluationAsync(id, evaluation);
            if (updatedEvaluation == null)
            {
                return NotFound();
            }
            return Ok(updatedEvaluation.ToEvaluationDto());
        }

        // DELETE: api/evaluations/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvaluation([FromRoute] Guid id, [FromQuery] Guid callerId)
        {
            var authorization = await _guard.RequireAsync(callerId, Permissions.EvaluationCreate);
            if (!authorization.Ok)
            {
                return authorization.ToActionResult();
            }

            var deleted = await _evaluationRepository.DeleteEvaluationAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
