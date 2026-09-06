namespace api.Dtos.Proposal
{
    public class ProposalDto
    {
        public Guid Id { get; set; } // Öneri ID

        public Guid UserId { get; set; } // Öneriyi oluşturan kullanıcı ID

        public string UserName { get; set; } = null!; // Öneriyi oluşturan kullanıcı adı

        public DateTime CreatedAt { get; set; } // Önerinin oluşturulma tarihi

        public string Title { get; set; } = null!; // Başlık

        public string Topic { get; set; } = null!; // Konu

        public string Purpose { get; set; } = null!; // Amaç

        public string Explanation { get; set; } = null!; // Açıklama

    }
}