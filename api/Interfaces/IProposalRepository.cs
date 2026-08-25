using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Interfaces
{
    public interface IProposalRepository
    {
        Task<IEnumerable<Models.Proposal>> GetAllProposalsAsync();
        Task<Models.Proposal?> GetProposalByIdAsync(int id);
        Task<Models.Proposal> CreateProposalAsync(Models.Proposal proposal);
        Task<Models.Proposal?> UpdateProposalAsync(int id, Models.Proposal proposal);
        Task<bool> DeleteProposalAsync(int id);
    }
}