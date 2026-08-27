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

        var permissions = new[]
        {
            new Permission { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Admin" },
            new Permission { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Reviewer" },
            new Permission { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Proposer" },
            new Permission { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "DepartmentManager" }
        };

        var users = new[]
        {
            CreateUser("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", "Efe", "Yilmaz", "5551112233", "REG-1001", "12345678901", true, permissions[0], permissions[1], permissions[2], permissions[3]),
            CreateUser("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb", "Can", "Kaya", "5552223344", "REG-1002", "12345678902", true, permissions[1], permissions[2]),
            CreateUser("cccccccc-cccc-cccc-cccc-cccccccccccc", "Ece", "Demir", "5553334455", "REG-1003", "12345678903", true, permissions[2]),
            CreateUser("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa04", "Deniz", "Arslan", "5554445566", "REG-1004", "12345678904", true, permissions[3], permissions[1]),
            CreateUser("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa05", "Selin", "Aydin", "5555556677", "REG-1005", "12345678905", true, permissions[2]),
            CreateUser("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa06", "Mert", "Koc", "5556667788", "REG-1006", "12345678906", true, permissions[2]),
            CreateUser("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa07", "Derya", "Celik", "5557778899", "REG-1007", "12345678907", true, permissions[1]),
            CreateUser("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa08", "Bora", "Sahin", "5558889900", "REG-1008", "12345678908", false, permissions[2]),
            CreateUser("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa09", "Irem", "Eren", "5559990011", "REG-1009", "12345678909", true, permissions[2]),
            CreateUser("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10", "Kerem", "Aksoy", "5560001122", "REG-1010", "12345678910", true, permissions[1], permissions[2]),
            CreateUser("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11", "Nehir", "Bulut", "5561112233", "REG-1011", "12345678911", true, permissions[2]),
            CreateUser("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa12", "Ozan", "Gunes", "5562223344", "REG-1012", "12345678912", true, permissions[3], permissions[2])
        };

        var proposals = new[]
        {
            CreateProposal("d1111111-1111-1111-1111-111111111111", users[2], -10, "Enerji Tasarruflu Aydinlatma", "Surdurulebilirlik", "Elektrik tuketimini azaltmak", "Ortak alanlarda hareket sensorlu LED sistemine gecis yapilarak enerji maliyetleri dusurulebilir."),
            CreateProposal("d2222222-2222-2222-2222-222222222222", users[0], -8, "Mentorluk Programi", "Kurum Ici Gelisim", "Deneyim aktarimini hizlandirmak", "Kidemli ekip uyeleri ile yeni calisanlari eslestiren 3 aylik bir mentorluk programi onerilmektedir."),
            CreateProposal("d3333333-3333-3333-3333-333333333333", users[4], -7, "Dijital Arsiv Projesi", "Dijital Donusum", "Belge arama suresini kisaltmak", "Kagit belgelerin aranabilir bir dijital arsivde toplanmasi is akisini hizlandiracaktir."),
            CreateProposal("d4444444-4444-4444-4444-444444444444", users[5], -6, "Esnek Calisma Saatleri", "Calisan Deneyimi", "Uretkenligi ve memnuniyeti artirmak", "Ekiplerin belirli saat araliklarinda esnek calisma yapmasi verimliligi destekleyebilir."),
            CreateProposal("d5555555-5555-5555-5555-555555555555", users[8], -5, "Geri Donusum Istasyonlari", "Cevre", "Atik ayrisimini kolaylastirmak", "Bina katlarina ayrik atik kutulari yerlestirilerek geri donusum orani artirilabilir."),
            CreateProposal("d6666666-6666-6666-6666-666666666666", users[10], -4, "Ic Iletisim Bulteni", "Iletisim", "Bilgi paylasimini duzenlemek", "Aylik kisa bir bulten ekiplerin basarilarini ve duyurulari tek yerde toplayabilir."),
            CreateProposal("d7777777-7777-7777-7777-777777777777", users[11], -3, "Toplanti Odasi Rezervasyonu", "Operasyon", "Oda kullanimini iyilestirmek", "Ortak takvim entegrasyonu ile toplanti odalarinin cakisarak rezerve edilmesi onlenebilir."),
            CreateProposal("d8888888-8888-8888-8888-888888888888", users[1], -2, "Yeni Calisan Oryantasyonu", "Insan Kaynaklari", "Ise uyum suresini kisaltmak", "Standart bir ilk hafta kontrol listesi yeni calisanlarin uyumunu kolaylastirabilir.")
        };

        var proposalFiles = proposals.SelectMany((proposal, index) => new[]
        {
            new ProposalFile { Id = Guid.Parse($"e{index + 1:0000000}-1111-1111-1111-111111111111"), Proposal = proposal, File = $"proposal_{index + 1:00}_brief.pdf" },
            new ProposalFile { Id = Guid.Parse($"e{index + 1:0000000}-2222-2222-2222-222222222222"), Proposal = proposal, File = $"proposal_{index + 1:00}_details.docx" }
        }).ToArray();

        var evaluations = new[]
        {
            CreateEvaluation("f1111111-1111-1111-1111-111111111111", users[1], proposals[0], "Uygulanabilir ve geri donus suresi kisa.", 9, true),
            CreateEvaluation("f2222222-2222-2222-2222-222222222222", users[0], proposals[0], "Butce ve bakim planinin netlestirilmesi gerekir.", 8, true),
            CreateEvaluation("f3333333-3333-3333-3333-333333333333", users[6], proposals[1], "Programin olculmesi icin hedefler eklenmeli.", 7, true),
            CreateEvaluation("f4444444-4444-4444-4444-444444444444", users[9], proposals[2], "Arama ve erisim kazanimi yuksek gorunuyor.", 9, true),
            CreateEvaluation("f5555555-5555-5555-5555-555555555555", users[3], proposals[3], "Ekipler arasi planlama icin ortak kurallar gerekli.", 6, true),
            CreateEvaluation("f6666666-6666-6666-6666-666666666666", users[0], proposals[4], "Uygulama maliyeti dusuk ve etkisi gorunur.", 10, true),
            CreateEvaluation("f7777777-7777-7777-7777-777777777777", users[1], proposals[5], "Icerik sorumlulari belirlenirse uygulanabilir.", 8, true),
            CreateEvaluation("f8888888-8888-8888-8888-888888888888", users[6], proposals[6], "Takvim entegrasyonu teknik olarak incelenmeli.", 7, true),
            CreateEvaluation("f9999999-9999-9999-9999-999999999999", users[0], proposals[7], "Oryantasyon sureci icin iyi bir baslangic.", 9, true),
            CreateEvaluation("fa000000-0000-0000-0000-000000000000", users[3], proposals[7], "Departman bazli ek adimlar eklenebilir.", 8, true)
        };

        context.Permissions.AddRange(permissions);
        context.Users.AddRange(users);
        context.Proposals.AddRange(proposals);
        context.ProposalFiles.AddRange(proposalFiles);
        context.Evaluations.AddRange(evaluations);

        await context.SaveChangesAsync(cancellationToken);
    }

    private static User CreateUser(string id, string name, string surname, string phone, string registrationNo, string governmentId, bool isActive, params Permission[] permissions)
    {
        var user = new User
        {
            Id = Guid.Parse(id),
            Name = name,
            Surname = surname,
            Phone = phone,
            RegistrationNo = registrationNo,
            GovernmentId = governmentId,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            IsActive = isActive
        };

        foreach (var permission in permissions)
        {
            user.Permissions.Add(permission);
        }

        return user;
    }

    private static Proposal CreateProposal(string id, User user, int daysAgo, string title, string topic, string purpose, string explanation)
    {
        return new Proposal
        {
            Id = Guid.Parse(id),
            UserId = user.Id,
            User = user,
            CreatedAt = DateTime.UtcNow.AddDays(daysAgo),
            Title = title,
            Topic = topic,
            Purpose = purpose,
            Explanation = explanation
        };
    }

    private static Evaluation CreateEvaluation(string id, User user, Proposal proposal, string comment, int score, bool isPositive)
    {
        return new Evaluation
        {
            Id = Guid.Parse(id),
            UserId = user.Id,
            User = user,
            ProposalId = proposal.Id,
            Proposal = proposal,
            Comment = comment,
            Score = score,
            IsPositive = isPositive
        };
    }
}