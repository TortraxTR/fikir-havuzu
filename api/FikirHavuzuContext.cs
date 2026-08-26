using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using api.Models;

namespace api;

public partial class FikirHavuzuContext : DbContext
{
    public FikirHavuzuContext(DbContextOptions<FikirHavuzuContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Evaluation> Evaluations { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Proposal> Proposals { get; set; }

    public virtual DbSet<ProposalFile> ProposalFiles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Evaluation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Yorum_pkey");

            entity.ToTable("Evaluation");

            entity.HasIndex(e => e.ProposalId, "IX_Değerlendirme_fikir_id");

            entity.HasIndex(e => e.UserId, "IX_Değerlendirme_yaratıcı_id");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.IsPositive).HasColumnName("is_positive");
            entity.Property(e => e.ProposalId).HasColumnName("proposal_id");
            entity.Property(e => e.Score)
                .HasColumnName("score");

            entity.HasOne(d => d.User).WithMany(p => p.Evaluations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Evaluation_creator_id_fkey");

            entity.HasOne(d => d.Proposal).WithMany(p => p.Evaluations)
                .HasForeignKey(d => d.ProposalId)
                .HasConstraintName("Evaluation_proposal_id_fkey");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("id");

            entity.ToTable("Permission");

            entity.HasIndex(e => e.Name, "YetkiAdı").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Proposal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Fikir_pkey");

            entity.ToTable("Proposal");

            entity.HasIndex(e => e.UserId, "IX_Fikir_yaratıcı_id");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Explanation)
                .HasMaxLength(8192)
                .HasColumnName("explanation");
            entity.Property(e => e.Purpose)
                .HasMaxLength(128)
                .HasColumnName("purpose");
            entity.Property(e => e.Title)
                .HasMaxLength(128)
                .HasColumnName("title");
            entity.Property(e => e.Topic)
                .HasMaxLength(128)
                .HasColumnName("topic");

            entity.HasOne(d => d.User).WithMany(p => p.Proposals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("yaratıcı_id");
        });

        modelBuilder.Entity<ProposalFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FikirDosya_pkey");

            entity.ToTable("ProposalFile");

            entity.HasIndex(e => e.ProposalId, "IX_FikirDosya_fikir_id");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.File).HasColumnName("file");
            entity.Property(e => e.ProposalId).HasColumnName("proposal_id");

            entity.HasOne(d => d.Proposal).WithMany(p => p.ProposalFiles)
                .HasForeignKey(d => d.ProposalId)
                .HasConstraintName("ProposalFile_proposal_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Kullanıcı_pkey");

            entity.ToTable("User");

            entity.HasIndex(e => e.GovernmentId, "Kullanıcı_kimlikNo_key").IsUnique();

            entity.HasIndex(e => e.RegistrationNo, "Kullanıcı_sicilNo_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.GovernmentId)
                .HasMaxLength(11)
                .HasColumnName("government_ID");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.RegistrationNo).HasColumnName("registration_no");
            entity.Property(e => e.Surname).HasColumnName("surname");

            entity.HasMany(d => d.Permissions).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserPermission",
                    r => r.HasOne<Permission>().WithMany()
                        .HasForeignKey("PermissionId")
                        .HasConstraintName("UserPermission_permission_id_fkey"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("UserPermission_user_id_fkey"),
                    j =>
                    {
                        j.HasKey("UserId", "PermissionId").HasName("KullanıcıYetki_pkey");
                        j.ToTable("UserPermission");
                        j.HasIndex(new[] { "PermissionId" }, "IX_KullanıcıYetki_yetki_id");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<Guid>("PermissionId").HasColumnName("permission_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
