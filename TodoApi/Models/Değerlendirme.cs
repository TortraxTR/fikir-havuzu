using System;
using System.Collections.Generic;

namespace TodoApi.Models;

public partial class Değerlendirme
{
    public int Id { get; set; }

    public int YaratıcıId { get; set; }

    public int FikirId { get; set; }

    public List<string>? Karar { get; set; }

    public string? Açıklama { get; set; }

    public int Puan { get; set; }

    public virtual Fikir Fikir { get; set; } = null!;

    public virtual Kullanıcı Yaratıcı { get; set; } = null!;
}
