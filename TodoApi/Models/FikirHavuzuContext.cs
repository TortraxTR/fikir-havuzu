using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

public partial class FikirHavuzuContext : DbContext
{
    public FikirHavuzuContext()
    {
    }

    public FikirHavuzuContext(DbContextOptions<FikirHavuzuContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Değerlendirme> Değerlendirmes { get; set; }

    public virtual DbSet<Fikir> Fikirs { get; set; }

    public virtual DbSet<FikirDosya> FikirDosyas { get; set; }

    public virtual DbSet<Kullanıcı> Kullanıcıs { get; set; }

    public virtual DbSet<Yetki> Yetkis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=FikirHavuzu;Username=postgres;Password=123456");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Değerlendirme>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Yorum_pkey");

            entity.ToTable("Değerlendirme");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Açıklama).HasColumnName("açıklama");
            entity.Property(e => e.FikirId).HasColumnName("fikir_id");
            entity.Property(e => e.Karar)
                .HasColumnType("character varying[]")
                .HasColumnName("karar");
            entity.Property(e => e.Puan)
                .ValueGeneratedOnAdd()
                .HasIdentityOptions(null, null, 0L, 5L, null, null)
                .HasColumnName("puan");
            entity.Property(e => e.YaratıcıId).HasColumnName("yaratıcı_id");

            entity.HasOne(d => d.Fikir).WithMany(p => p.Değerlendirmes)
                .HasForeignKey(d => d.FikirId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fikir_id");

            entity.HasOne(d => d.Yaratıcı).WithMany(p => p.Değerlendirmes)
                .HasForeignKey(d => d.YaratıcıId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("yaratıcı_id");
        });

        modelBuilder.Entity<Fikir>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Fikir_pkey");

            entity.ToTable("Fikir");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, 0L, null, null, null)
                .HasColumnName("id");
            entity.Property(e => e.Amaç)
                .HasMaxLength(128)
                .HasColumnName("amaç");
            entity.Property(e => e.Açıklama)
                .HasMaxLength(8192)
                .HasColumnName("açıklama");
            entity.Property(e => e.Başlık)
                .HasMaxLength(128)
                .HasColumnName("başlık");
            entity.Property(e => e.Konu)
                .HasMaxLength(128)
                .HasColumnName("konu");
            entity.Property(e => e.YaratıcıId).HasColumnName("yaratıcı_id");
            entity.Property(e => e.YaratılmaTarihi)
                .HasDefaultValueSql("now()")
                .HasColumnName("yaratılma_tarihi");

            entity.HasOne(d => d.Yaratıcı).WithMany(p => p.Fikirs)
                .HasForeignKey(d => d.YaratıcıId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("yaratıcı_id");
        });

        modelBuilder.Entity<FikirDosya>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FikirDosya_pkey");

            entity.ToTable("FikirDosya");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, 0L, null, null, null)
                .HasColumnName("id");
            entity.Property(e => e.Dosya).HasColumnName("dosya");
            entity.Property(e => e.FikirId).HasColumnName("fikir_id");

            entity.HasOne(d => d.Fikir).WithMany(p => p.FikirDosyas)
                .HasForeignKey(d => d.FikirId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fikir_id");
        });

        modelBuilder.Entity<Kullanıcı>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Kullanıcı_pkey");

            entity.ToTable("Kullanıcı");

            entity.HasIndex(e => e.KimlikNo, "Kullanıcı_kimlikNo_key").IsUnique();

            entity.HasIndex(e => e.SicilNo, "Kullanıcı_sicilNo_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, 0L, null, null, null)
                .HasColumnName("id");
            entity.Property(e => e.Ad).HasColumnName("ad");
            entity.Property(e => e.Aktif)
                .HasDefaultValue(true)
                .HasColumnName("aktif");
            entity.Property(e => e.KimlikNo)
                .HasMaxLength(11)
                .HasColumnName("kimlikNo");
            entity.Property(e => e.SicilNo).HasColumnName("sicilNo");
            entity.Property(e => e.Soyad).HasColumnName("soyad");
            entity.Property(e => e.TelefonNo)
                .HasMaxLength(15)
                .HasColumnName("telefonNo");
            entity.Property(e => e.ŞifreHash).HasColumnName("şifre_hash");

            entity.HasMany(d => d.Yetkis).WithMany(p => p.Kullanıcıs)
                .UsingEntity<Dictionary<string, object>>(
                    "KullanıcıYetki",
                    r => r.HasOne<Yetki>().WithMany()
                        .HasForeignKey("YetkiId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Yetki"),
                    l => l.HasOne<Kullanıcı>().WithMany()
                        .HasForeignKey("KullanıcıId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Kullanıcı"),
                    j =>
                    {
                        j.HasKey("KullanıcıId", "YetkiId").HasName("KullanıcıYetki_pkey");
                        j.ToTable("KullanıcıYetki");
                        j.IndexerProperty<int>("KullanıcıId").HasColumnName("kullanıcı_id");
                        j.IndexerProperty<int>("YetkiId").HasColumnName("yetki_id");
                    });
        });

        modelBuilder.Entity<Yetki>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("id");

            entity.ToTable("Yetki");

            entity.HasIndex(e => e.Isim, "YetkiAdı").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, 0L, null, null, null)
                .HasColumnName("id");
            entity.Property(e => e.Isim).HasColumnName("isim");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
