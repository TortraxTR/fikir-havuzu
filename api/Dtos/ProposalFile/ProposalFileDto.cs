using System;

namespace api.Dtos.ProposalFile
{
    public class ProposalFileDto
    {
        public Guid Id { get; set; } // Dosya ID

        public Guid ProposalId { get; set; } // Dosyanın ait olduğu öneri ID

        public string FileName { get; set; } = null!; // Dosya adı

        public string ContentType { get; set; } = null!;

        public long SizeBytes { get; set; }
    }
}
