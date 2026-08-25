using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Proposal
{
    public class CreateProposalRequestDto
    {
        public int CreatorId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Title { get; set; } = null!;

        public string Topic { get; set; } = null!;

        public string Purpose { get; set; } = null!;

        public string Explanation { get; set; } = null!;

    }
}