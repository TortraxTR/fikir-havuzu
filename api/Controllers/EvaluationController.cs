using api.Interfaces;
using api.Models;
using api.Mappers.EvaluationMappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/evaluations")]
    [ApiController]
    public class EvaluationController : ControllerBase
    {
        private readonly IEvaluationRepository _evaluationRepository;

        public EvaluationController(IEvaluationRepository evaluationRepository)
        {
            _evaluationRepository = evaluationRepository;
        }

        // GET: api/evaluations
        [HttpGet]
        public async Task<IActionResult> GetEvaluations()
        {
            var evaluations = await _evaluationRepository.GetAllEvaluationsAsync();
            return Ok(evaluations.Select(evaluation => evaluation.ToEvaluationDto()));
        }

        // GET: api/evaluations/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvaluation([FromRoute] Guid id)
        {
            var evaluation = await _evaluationRepository.GetEvaluationByIdAsync(id);
            if (evaluation == null)
            {
                return NotFound();
            }
            return Ok(evaluation.ToEvaluationDto());
        }

        // POST: api/evaluations
        [HttpPost]

        public async Task<IActionResult> CreateEvaluation([FromBody] Dtos.Evaluation.CreateEvaluationRequestDto evaluationDto)
        {
            var evaluation = evaluationDto.ToEvaluation();
            var createdEvaluation = await _evaluationRepository.CreateEvaluationAsync(evaluation);
            return CreatedAtAction(nameof(GetEvaluation), new { id = createdEvaluation.Id }, createdEvaluation.ToEvaluationDto());
        }

        // PUT: api/evaluations/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvaluation([FromRoute] Guid id, [FromBody] Dtos.Evaluation.UpdateEvaluationRequestDto evaluationDto)
        {
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
        public async Task<IActionResult> DeleteEvaluation([FromRoute] Guid id)
        {
            var deleted = await _evaluationRepository.DeleteEvaluationAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}