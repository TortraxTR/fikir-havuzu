using System;
using System.Collections.Generic;

namespace TodoApi.Models;

public partial class Kullanıcı
{
    public int Id { get; set; }

    public string Ad { get; set; } = null!;

    public string TelefonNo { get; set; } = null!;

    public string SicilNo { get; set; } = null!;

    public string KimlikNo { get; set; } = null!;

    public string ŞifreHash { get; set; } = null!;

    public string Soyad { get; set; } = null!;

    public bool Aktif { get; set; }

    public virtual ICollection<Değerlendirme> Değerlendirmes { get; set; } = new List<Değerlendirme>();

    public virtual ICollection<Fikir> Fikirs { get; set; } = new List<Fikir>();

    public virtual ICollection<Yetki> Yetkis { get; set; } = new List<Yetki>();
}
