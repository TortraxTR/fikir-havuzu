using System;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.ProposalFile
{
    public class UpdateProposalFileRequestDto
    {
        [Required]
        public Guid ProposalId { get; set; } // Dosyanın ait olduğu öneri ID

        [Required]
        [StringLength(1000, ErrorMessage = "File name/path cannot exceed 1000 characters.")]
        public string File { get; set; } = null!; // Dosya adı / yolu
    }
}
