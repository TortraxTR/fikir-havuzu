using System;
using System.Collections.Generic;

namespace api.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string RegistrationNo { get; set; } = null!;

    public string GovernmentId { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();

    public virtual ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
