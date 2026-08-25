using System;
using System.Collections.Generic;

namespace TodoApi.Models;

public partial class Evaluation
{
    public int Id { get; set; }

    public int CreatorId { get; set; }

    public int ProposalId { get; set; }

    public List<string>? Decision { get; set; }

    public string? Comment { get; set; }

    public int Score { get; set; }

    public virtual User Creator { get; set; } = null!;

    public virtual Proposal Proposal { get; set; } = null!;
}
