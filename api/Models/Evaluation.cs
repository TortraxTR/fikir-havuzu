using System;
using System.Collections.Generic;

namespace api.Models;

public partial class Evaluation
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid ProposalId { get; set; }

    public string? Comment { get; set; }

    public int Score { get; set; }

    public bool IsPositive { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Proposal Proposal { get; set; } = null!;
}
