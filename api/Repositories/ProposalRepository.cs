using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return await _context.Proposals.ToListAsync();
        }

        public async Task<Proposal?> GetProposalByIdAsync(Guid id)
        {
            return await _context.Proposals.FindAsync(id);
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