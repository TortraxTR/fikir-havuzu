namespace api.Dtos.Evaluation
{
    public class EvaluationDto
    {
        public Guid Id { get; set; } // Değerlendirme ID

        public Guid UserId { get; set; } // Değerlendirmeyi yapan kullanıcı ID

        public Guid ProposalId { get; set; } // Değerlendirilen öneri ID

        public int Score { get; set; } // Değerlendirme puanı

        public string? Comment { get; set; } // Değerlendirme yorumu

        public bool IsPositive { get; set; } // Değerlendirme olumlu mu?
    }
}