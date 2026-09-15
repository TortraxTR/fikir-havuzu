using System;
using System.Collections.Generic;

namespace api.Models;

public partial class ProposalFile
{
    public Guid Id { get; set; }

    public Guid ProposalId { get; set; }

    public string FileName { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public string StorageKey { get; set; } = null!;

    public long SizeBytes { get; set; }

    public virtual Proposal Proposal { get; set; } = null!;
}
