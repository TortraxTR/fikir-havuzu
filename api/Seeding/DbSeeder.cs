using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Seeding;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FikirHavuzuContext>();

        await context.Database.MigrateAsync(cancellationToken);

        if (await context.Users.AnyAsync(cancellationToken))
        {
            return;
        }

        var adminPermission = new Permission
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Admin"
        };

        var reviewerPermission = new Permission
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Name = "Reviewer"
        };

        var proposerPermission = new Permission
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Name = "Proposer"
        };

        var adminUser = new User
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Name = "Efe",
            Surname = "Yilmaz",
            Phone = "+905551112233",
            RegistrationNo = "REG-1001",
            GovernmentId = "12345678901",
            PasswordHash = "seeded-admin-password-hash",
            IsActive = true,
            Permissions = { adminPermission, reviewerPermission, proposerPermission }
        };

        var reviewerUser = new User
        {
            Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Name = "Can",
            Surname = "Kaya",
            Phone = "+905552223344",
            RegistrationNo = "REG-1002",
            GovernmentId = "12345678902",
            PasswordHash = "seeded-reviewer-password-hash",
            IsActive = true,
            Permissions = { reviewerPermission }
        };

        var proposerUser = new User
        {
            Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            Name = "Ece",
            Surname = "Demir",
            Phone = "+905553334455",
            RegistrationNo = "REG-1003",
            GovernmentId = "12345678903",
            PasswordHash = "seeded-proposer-password-hash",
            IsActive = true,
            Permissions = { proposerPermission }
        };

        var proposal1 = new Proposal
        {
            Id = Guid.Parse("d1111111-1111-1111-1111-111111111111"),
            Creator = proposerUser,
            CreatedAt = DateTime.UtcNow.AddDays(-10),
            Title = "Enerji Tasarruflu Aydinlatma",
            Topic = "Surdurulebilirlik",
            Purpose = "Elektrik tuketimini azaltmak",
            Explanation = "Ortak alanlarda hareket sensorlu LED sistemine gecis yapilarak enerji maliyetleri dusurulebilir."
        };

        var proposal2 = new Proposal
        {
            Id = Guid.Parse("d2222222-2222-2222-2222-222222222222"),
            Creator = adminUser,
            CreatedAt = DateTime.UtcNow.AddDays(-5),
            Title = "Mentorluk Programi",
            Topic = "Kurum Ici Gelisim",
            Purpose = "Deneyim aktarimini hizlandirmak",
            Explanation = "Kidemli ekip uyeleri ile yeni calisanlari eslestiren 3 aylik bir mentorluk programi onerilmektedir."
        };

        var proposalFile1 = new ProposalFile
        {
            Id = Guid.Parse("e1111111-1111-1111-1111-111111111111"),
            Proposal = proposal1,
            File = "enerji_tasarrufu_analizi.pdf"
        };

        var proposalFile2 = new ProposalFile
        {
            Id = Guid.Parse("e2222222-2222-2222-2222-222222222222"),
            Proposal = proposal2,
            File = "mentorluk_program_taslagi.docx"
        };

        var evaluation1 = new Evaluation
        {
            Id = Guid.Parse("f1111111-1111-1111-1111-111111111111"),
            Creator = reviewerUser,
            Proposal = proposal1,
            Comment = "Uygulanabilir ve geri donus suresi kisa.",
            IsPositive = true
        };

        var evaluation2 = new Evaluation
        {
            Id = Guid.Parse("f2222222-2222-2222-2222-222222222222"),
            Creator = adminUser,
            Proposal = proposal2,
            Comment = "Kaynak planlamasi netlestirilirse etkisi yuksek olur.",
            IsPositive = true
        };

        context.Permissions.AddRange(adminPermission, reviewerPermission, proposerPermission);
        context.Users.AddRange(adminUser, reviewerUser, proposerUser);
        context.Proposals.AddRange(proposal1, proposal2);
        context.ProposalFiles.AddRange(proposalFile1, proposalFile2);
        context.Evaluations.AddRange(evaluation1, evaluation2);

        await context.SaveChangesAsync(cancellationToken);
    }
}