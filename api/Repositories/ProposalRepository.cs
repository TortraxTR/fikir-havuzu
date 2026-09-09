using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class ProposalRepository : IProposalRepository
    {
        private readonly FikirHavuzuContext _context;

        public ProposalRepository(FikirHavuzuContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Proposal>> GetAllProposalsAsync()
        {
            return await _context.Proposals.Include(p => p.User).ToListAsync();
        }

        public async Task<Proposal?> GetProposalByIdAsync(Guid id)
        {
            return await _context.Proposals
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Proposal>> GetProposalsByUserIdAsync(Guid userId)
        {
            return await _context.Proposals
                .Include(p => p.User)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Evaluation>> GetProposalEvaluationsAsync(Guid proposalId)
        {       
            return await _context.Evaluations
                .Where(e => e.ProposalId == proposalId)
                .ToListAsync();
        }

        public async Task<Proposal> CreateProposalAsync(Proposal proposal)
        {
            await _context.Proposals.AddAsync(proposal);
            await _context.SaveChangesAsync();
            return proposal;
        }

        public async Task<Proposal?> UpdateProposalAsync(Guid id, Proposal proposal)
        {
            var existingProposal = await _context.Proposals.FindAsync(id);
            if (existingProposal == null)
            {
                return null;
            }
            existingProposal.Title = proposal.Title;
            existingProposal.Topic = proposal.Topic;
            existingProposal.Purpose = proposal.Purpose;
            existingProposal.Explanation = proposal.Explanation;
            
            await _context.SaveChangesAsync();
            return existingProposal;
        }

        public async Task<bool> DeleteProposalAsync(Guid id)
        {
            var proposal = await _context.Proposals.FindAsync(id);
            if (proposal == null)
            {
                return false;
            }

            _context.Proposals.Remove(proposal);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}