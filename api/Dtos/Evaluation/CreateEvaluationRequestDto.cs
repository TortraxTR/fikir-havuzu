namespace api.Dtos.Evaluation
{
    public class CreateEvaluationRequestDto
    {
        public Guid UserId { get; set; } // Değerlendirmeyi yapan kullanıcı ID

        public Guid ProposalId { get; set; } // Değerlendirilen öneri ID

        public string? Comment { get; set; } // Değerlendirme yorumu (isteğe bağlı)

        public int Score { get; set; } // Değerlendirme puanı

        public bool IsPositive { get; set; } // Değerlendirme olumlu mu?
    }
}