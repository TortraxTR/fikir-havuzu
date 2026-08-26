using System;
using System.Collections.Generic;

namespace api.Models;

public partial class ProposalFile
{
    public Guid Id { get; set; }

    public Guid ProposalId { get; set; }

    public string File { get; set; } = null!;

    public virtual Proposal Proposal { get; set; } = null!;
}
