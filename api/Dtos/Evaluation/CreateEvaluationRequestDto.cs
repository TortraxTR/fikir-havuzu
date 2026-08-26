using System;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Evaluation
{
    public class CreateEvaluationRequestDto
    {
        [Required]
        public Guid UserId { get; set; } // Değerlendirmeyi yapan kullanıcı ID

        [Required]
        public Guid ProposalId { get; set; } // Değerlendirilen öneri ID

        [StringLength(4000, ErrorMessage = "Comment cannot exceed 4000 characters.")]
        public string? Comment { get; set; } // Değerlendirme yorumu (isteğe bağlı)

        [Range(0, 5, ErrorMessage = "Score must be between 0 and 5.")]
        public int Score { get; set; } // Değerlendirme puanı

        [Required]
        public bool IsPositive { get; set; } // Değerlendirme olumlu mu?
    }
}