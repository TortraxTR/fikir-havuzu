using System;
using System.Collections.Generic;

namespace api.Models;

public partial class Proposal
{
    public int Id { get; set; }

    public int CreatorId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Title { get; set; } = null!;

    public string Topic { get; set; } = null!;

    public string Purpose { get; set; } = null!;

    public string Explanation { get; set; } = null!;

    public virtual User Creator { get; set; } = null!;

    public virtual ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();

    public virtual ICollection<ProposalFile> ProposalFiles { get; set; } = new List<ProposalFile>();
}
