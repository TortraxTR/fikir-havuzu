using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Proposal
{
    public class UpdateProposalRequestDto
    {
        [Required]
        [StringLength(128, ErrorMessage = "Title cannot exceed 128 characters.")]
        public string Title { get; set; } = null!; // Başlık

        [Required]
        [StringLength(128, ErrorMessage = "Topic cannot exceed 128 characters.")]
        public string Topic { get; set; } = null!; // Konu

        [Required]
        [StringLength(128, ErrorMessage = "Purpose cannot exceed 128 characters.")]
        public string Purpose { get; set; } = null!; // Amaç

        [Required]
        [StringLength(8192, ErrorMessage = "Explanation cannot exceed 8192 characters.")]
        public string Explanation { get; set; } = null!; // Açıklama

    }
}