using System;

namespace api.Dtos.ProposalFile
{
    public class ProposalFileDto
    {
        public Guid Id { get; set; } // Dosya ID

        public Guid ProposalId { get; set; } // Dosyanın ait olduğu öneri ID

        public string File { get; set; } = null!; // Dosya adı / yolu
    }
}
