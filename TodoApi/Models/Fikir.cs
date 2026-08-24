using System;
using System.Collections.Generic;

namespace TodoApi.Models;

public partial class Fikir
{
    public int Id { get; set; }

    public int YaratıcıId { get; set; }

    public DateTime YaratılmaTarihi { get; set; }

    public string Başlık { get; set; } = null!;

    public string Konu { get; set; } = null!;

    public string Amaç { get; set; } = null!;

    public string Açıklama { get; set; } = null!;

    public virtual ICollection<Değerlendirme> Değerlendirmes { get; set; } = new List<Değerlendirme>();

    public virtual ICollection<FikirDosya> FikirDosyas { get; set; } = new List<FikirDosya>();

    public virtual Kullanıcı Yaratıcı { get; set; } = null!;
}
