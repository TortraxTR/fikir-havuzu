namespace api.Dtos.Proposal
{
    public class UpdateProposalRequestDto
    {
        public string Title { get; set; } = null!; // Başlık

        public string Topic { get; set; } = null!; // Konu

        public string Purpose { get; set; } = null!; // Amaç

        public string Explanation { get; set; } = null!; // Açıklama

    }
}