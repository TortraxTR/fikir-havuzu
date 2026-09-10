using api.Common;
using api.Dtos.Evaluation;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/evaluations")]
    [ApiController]
    public class EvaluationController : ControllerBase
    {
        private readonly IEvaluationService _evaluations;

        public EvaluationController(IEvaluationService evaluations)
        {
            _evaluations = evaluations;
        }

        // GET: api/evaluations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EvaluationDto>>> GetEvaluations([FromQuery] Guid callerId)
        {
            var result = await _evaluations.GetAllAsync(callerId);
            return result.ToActionResult();
        }

        // GET: api/evaluations/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EvaluationDto>> GetEvaluation([FromRoute] Guid id, [FromQuery] Guid callerId)
        {
            var result = await _evaluations.GetByIdAsync(id, callerId);
            return result.ToActionResult();
        }

        // POST: api/evaluations
        [HttpPost]
        public async Task<ActionResult<EvaluationDto>> CreateEvaluation([FromBody] CreateEvaluationRequestDto evaluationDto)
        {
            var result = await _evaluations.CreateAsync(evaluationDto);
            if (!result.Ok)
            {
                return result.Error();
            }

            return CreatedAtAction(
                nameof(GetEvaluation),
                new { id = result.Value!.Id, callerId = evaluationDto.UserId },
                result.Value);
        }

        // PUT: api/evaluations/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<EvaluationDto>> UpdateEvaluation([FromRoute] Guid id, [FromBody] UpdateEvaluationRequestDto evaluationDto)
        {
            var result = await _evaluations.UpdateAsync(id, evaluationDto);
            return result.ToActionResult();
        }

        // DELETE: api/evaluations/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvaluation([FromRoute] Guid id, [FromQuery] Guid callerId)
        {
            var result = await _evaluations.DeleteAsync(id, callerId);
            return result.ToActionResult();
        }
    }
}
