using System;
using System.Collections.Generic;

namespace TodoApi.Models;

public partial class Yetki
{
    public int Id { get; set; }

    public string? Isim { get; set; }

    public virtual ICollection<Kullanıcı> Kullanıcıs { get; set; } = new List<Kullanıcı>();
}
