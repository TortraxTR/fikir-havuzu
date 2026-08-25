using System;
using System.Collections.Generic;

namespace TodoApi.Models;

public partial class ProposalFile
{
    public int Id { get; set; }

    public int ProposalId { get; set; }

    public string File { get; set; } = null!;

    public virtual Proposal Proposal { get; set; } = null!;
}
