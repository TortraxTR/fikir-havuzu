using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class ProposalFileRepository : IProposalFileRepository
    {
        private readonly FikirHavuzuContext _context;

        public ProposalFileRepository(FikirHavuzuContext context)
        {
            _context = context;
        }

        public async Task<ProposalFile?> GetProposalFileByIdAsync(Guid id)
        {
            return await _context.ProposalFiles.FindAsync(id);
        }

        public async Task<IEnumerable<ProposalFile>> GetProposalFilesByProposalIdAsync(Guid proposalId)
        {
            return await _context.ProposalFiles
                .Where(pf => pf.ProposalId == proposalId)
                .ToListAsync();
        }

        public async Task<ProposalFile> CreateProposalFileAsync(ProposalFile proposalFile)
        {
            await _context.ProposalFiles.AddAsync(proposalFile);
            await _context.SaveChangesAsync();
            return proposalFile;
        }
    }
}
