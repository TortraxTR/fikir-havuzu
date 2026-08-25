using System;
using System.Collections.Generic;

namespace api.Models;

public partial class Permission
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
