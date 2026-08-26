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

        public async Task<IEnumerable<ProposalFile>> GetAllProposalFilesAsync()
        {
            return await _context.ProposalFiles.ToListAsync();
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

        public async Task<ProposalFile?> UpdateProposalFileAsync(Guid id, ProposalFile proposalFile)
        {
            var existingFile = await _context.ProposalFiles.FindAsync(id);
            if (existingFile == null)
            {
                return null;
            }

            existingFile.ProposalId = proposalFile.ProposalId;
            existingFile.File = proposalFile.File;

            await _context.SaveChangesAsync();
            return existingFile;
        }

        public async Task<bool> DeleteProposalFileAsync(Guid id)
        {
            var proposalFile = await _context.ProposalFiles.FindAsync(id);
            if (proposalFile == null)
            {
                return false;
            }

            _context.ProposalFiles.Remove(proposalFile);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
