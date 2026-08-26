using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class EvaluationRepository : IEvaluationRepository
    {
        private readonly FikirHavuzuContext _context;

        public EvaluationRepository(FikirHavuzuContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Evaluation>> GetAllEvaluationsAsync()
        {
            return await _context.Evaluations.ToListAsync();
        }

        public async Task<Evaluation?> GetEvaluationByIdAsync(Guid id)
        {
            return await _context.Evaluations.FindAsync(id);
        }

        public async Task<IEnumerable<Evaluation>> GetEvaluationsByProposalIdAsync(Guid proposalId)
        {
            return await _context.Evaluations
                .Where(e => e.ProposalId == proposalId)
                .ToListAsync();
        }
       
        public async Task<IEnumerable<Evaluation>> GetEvaluationsByUserIdAsync(Guid userId)
        {
            return await _context.Evaluations
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task<Evaluation> CreateEvaluationAsync(Evaluation evaluation)
        {
            await _context.Evaluations.AddAsync(evaluation);
            await _context.SaveChangesAsync();
            return evaluation;
        }

        public async Task<Evaluation?> UpdateEvaluationAsync(Guid id, Evaluation evaluation)
        {
            var existingEvaluation = await _context.Evaluations.FindAsync(id);
            if (existingEvaluation == null)
            {
                return null;
            }

            existingEvaluation.UserId = evaluation.UserId;
            existingEvaluation.ProposalId = evaluation.ProposalId;
            existingEvaluation.Comment = evaluation.Comment;
            existingEvaluation.Score = evaluation.Score;
            existingEvaluation.IsPositive = evaluation.IsPositive;

            await _context.SaveChangesAsync();
            return existingEvaluation;
        }

        public async Task<bool> DeleteEvaluationAsync(Guid id)
        {
            var evaluation = await _context.Evaluations.FindAsync(id);
            if (evaluation == null)
            {
                return false;
            }
            _context.Evaluations.Remove(evaluation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}