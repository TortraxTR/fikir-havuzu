using api.Dtos.Evaluation;
using api.Models;

namespace api.Mappers.EvaluationMappers;

public static class EvaluationMapper
{
    public static EvaluationDto ToEvaluationDto(this Evaluation evaluation)
    {
        return new EvaluationDto
        {
            Id = evaluation.Id,
            UserId = evaluation.UserId,
            ProposalId = evaluation.ProposalId,
            Comment = evaluation.Comment,
            Score = evaluation.Score,
            IsPositive = evaluation.IsPositive
        };
    }

    public static Evaluation ToEvaluation(this CreateEvaluationRequestDto evaluationDto)
    {
        return new Evaluation
        {
            UserId = evaluationDto.UserId,
            ProposalId = evaluationDto.ProposalId,
            Comment = evaluationDto.Comment,
            Score = evaluationDto.Score,
            IsPositive = evaluationDto.IsPositive
        };
    }

    public static Evaluation ToEvaluation(this UpdateEvaluationRequestDto evaluationDto)
    {
        return new Evaluation
        {
            UserId = evaluationDto.UserId,
            ProposalId = evaluationDto.ProposalId,
            Comment = evaluationDto.Comment,
            Score = evaluationDto.Score,
            IsPositive = evaluationDto.IsPositive
        };
    }
}