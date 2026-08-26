using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Proposal
{
    public class ProposalDto
    {
        public Guid Id { get; set; } // Öneri ID

        public Guid CreatorId { get; set; } // Öneriyi oluşturan kullanıcı ID

        public DateTime CreatedAt { get; set; } // Önerinin oluşturulma tarihi

        public string Title { get; set; } = null!; // Başlık

        public string Topic { get; set; } = null!; // Konu

        public string Purpose { get; set; } = null!; // Amaç

        public string Explanation { get; set; } = null!; // Açıklama

    }
}