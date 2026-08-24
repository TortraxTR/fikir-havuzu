using System;
using System.Collections.Generic;

namespace TodoApi.Models;

public partial class FikirDosya
{
    public int Id { get; set; }

    public int FikirId { get; set; }

    public string Dosya { get; set; } = null!;

    public virtual Fikir Fikir { get; set; } = null!;
}
