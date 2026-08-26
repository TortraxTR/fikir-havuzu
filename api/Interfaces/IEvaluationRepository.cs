namespace api.Interfaces
{
    public interface IEvaluationRepository
    {
        Task<IEnumerable<Models.Evaluation>> GetAllEvaluationsAsync();
        Task<Models.Evaluation?> GetEvaluationByIdAsync(Guid id);
        Task<IEnumerable<Models.Evaluation>> GetEvaluationsByProposalIdAsync(Guid proposalId);
        Task<IEnumerable<Models.Evaluation>> GetEvaluationsByUserIdAsync(Guid userId);

        Task<Models.Evaluation> CreateEvaluationAsync(Models.Evaluation evaluation);
        Task<Models.Evaluation?> UpdateEvaluationAsync(Guid id, Models.Evaluation evaluation);
        Task<bool> DeleteEvaluationAsync(Guid id);
    }
}