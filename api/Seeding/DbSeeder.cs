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
            new Permission { Id = Guid.NewGuid(), Code = "USER_MANAGEMENT", Name = "Kullanıcı Yönetimi" },
            new Permission { Id = Guid.NewGuid(), Code = "PERMISSION_MANAGEMENT", Name = "Yetki Yönetimi" },
            new Permission { Id = Guid.NewGuid(), Code = "PROPOSAL_CREATE", Name = "Öneri Oluşturma" },
            new Permission { Id = Guid.NewGuid(), Code = "EVALUATION_CREATE", Name = "Değerlendirme Oluşturma" }
        };

        var users = new[]
        {
            CreateUser("Efe", "Yılmaz", "efe.yilmaz@example.com", "5551112233", "REG-1001", "12345678901", true, permissions[0], permissions[1], permissions[2], permissions[3]),
            CreateUser("Can", "Kaya", "can.kaya@example.com", "5552223344", "REG-1002", "12345678902", true, permissions[1], permissions[2]),
            CreateUser("Ece", "Demir", "ece.demir@example.com", "5553334455", "REG-1003", "12345678903", true, permissions[2]),
            CreateUser("Deniz", "Arslan", "deniz.arslan@example.com", "5554445566", "REG-1004", "12345678904", true, permissions[3], permissions[1]),
            CreateUser("Selin", "Aydın", "selin.aydin@example.com", "5555556677", "REG-1005", "12345678905", true, permissions[2]),
            CreateUser("Mert", "Koç", "mert.koc@example.com", "5556667788", "REG-1006", "12345678906", true, permissions[2]),
            CreateUser("Derya", "Çelik", "derya.celik@example.com", "5557778899", "REG-1007", "12345678907", true, permissions[1]),
            CreateUser("Bora", "Şahin", "bora.sahin@example.com", "5558889900", "REG-1008", "12345678908", false, permissions[2]),
            CreateUser("İrem", "Eren", "irem.eren@example.com", "5559990011", "REG-1009", "12345678909", true, permissions[2]),
            CreateUser("Kerem", "Aksoy", "kerem.aksoy@example.com", "5560001122", "REG-1010", "12345678910", true, permissions[1], permissions[2]),
            CreateUser("Nehir", "Bulut", "nehir.bulut@example.com", "5561112233", "REG-1011", "12345678911", true, permissions[2]),
            CreateUser("Ozan", "Güneş", "ozan.gunes@example.com", "5562223344", "REG-1012", "12345678912", true, permissions[3], permissions[2])
        };

        var proposals = new[]
        {
            CreateProposal(users[2], -10, "Enerji Tasarruflu Aydınlatma", "Sürdürülebilirlik", "Elektrik tüketimini azaltmak", "Ortak alanlarda hareket sensörlü LED sistemine geçiş yapılarak enerji maliyetleri düşürülebilir."),
            CreateProposal(users[0], -8, "Mentorluk Programı", "Kurum İçi Gelişim", "Deneyim aktarımını hızlandırmak", "Kıdemli ekip üyeleri ile yeni çalışanları eşleştiren 3 aylık bir mentorluk programı önerilmektedir."),
            CreateProposal(users[4], -7, "Dijital Arşiv Projesi", "Dijital Dönüşüm", "Belge arama süresini kısaltmak", "Kağıt belgelerin aranabilir bir dijital arşivde toplanması iş akışını hızlandıracaktır."),
            CreateProposal(users[5], -6, "Esnek Çalışma Saatleri", "Çalışan Deneyimi", "Üretkenliği ve memnuniyeti artırmak", "Ekiplerin belirli saat aralıklarında esnek çalışma yapması verimliliği destekleyebilir."),
            CreateProposal(users[8], -5, "Geri Dönüşüm İstasyonları", "Çevre", "Atık ayrışımını kolaylaştırmak", "Bina katlarına ayrık atık kutuları yerleştirilerek geri dönüşüm oranı artırılabilir."),
            CreateProposal(users[10], -4, "İç İletişim Bülteni", "İletişim", "Bilgi paylaşımını düzenlemek", "Aylık kısa bir bülten ekiplerin başarılarını ve duyuruları tek yerde toplayabilir."),
            CreateProposal(users[11], -3, "Toplantı Odası Rezervasyonu", "Operasyon", "Oda kullanımını iyileştirmek", "Ortak takvim entegrasyonu ile toplantı odalarının çakışarak rezerve edilmesi önlenebilir."),
            CreateProposal(users[1], -2, "Yeni Çalışan Oryantasyonu", "İnsan Kaynakları", "İşe uyum süresini kısaltmak", "Standart bir ilk hafta kontrol listesi yeni çalışanların uyumunu kolaylaştırabilir.")
        };

        var proposalFiles = proposals.SelectMany((proposal, index) => new[]
        {
            new ProposalFile { Id = Guid.NewGuid(), Proposal = proposal, File = $"proposal_{index + 1:00}_brief.pdf" },
            new ProposalFile { Id = Guid.NewGuid(), Proposal = proposal, File = $"proposal_{index + 1:00}_details.docx" }
        }).ToArray();

        var evaluations = new[]
        {
            CreateEvaluation(users[1], proposals[0], "Uygulanabilir ve geri dönüş süresi kısa.", 9, true),
            CreateEvaluation(users[0], proposals[0], "Bütçe ve bakım planının netleştirilmesi gerekir.", 8, true),
            CreateEvaluation(users[6], proposals[1], "Programın ölçülmesi için hedefler eklenmeli.", 7, true),
            CreateEvaluation(users[9], proposals[2], "Arama ve erişim kazanımı yüksek görünüyor.", 9, true),
            CreateEvaluation(users[3], proposals[3], "Ekipler arası planlama için ortak kurallar gerekli.", 6, true),
            CreateEvaluation(users[0], proposals[4], "Uygulama maliyeti düşük ve etkisi görünür.", 10, true),
            CreateEvaluation(users[1], proposals[5], "İçerik sorumluları belirlenirse uygulanabilir.", 8, true),
            CreateEvaluation(users[6], proposals[6], "Takvim entegrasyonu teknik olarak incelenmeli.", 7, true),
            CreateEvaluation(users[0], proposals[7], "Oryantasyon süreci için iyi bir başlangıç.", 9, true),
            CreateEvaluation(users[3], proposals[7], "Departman bazlı ek adımlar eklenebilir.", 8, true)
        };

        context.Permissions.AddRange(permissions);
        context.Users.AddRange(users);
        context.Proposals.AddRange(proposals);
        context.ProposalFiles.AddRange(proposalFiles);
        context.Evaluations.AddRange(evaluations);

        await context.SaveChangesAsync(cancellationToken);
    }

    private static User CreateUser(string name, string surname, string email, string phone, string registrationNo, string governmentId, bool isActive, params Permission[] permissions)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Surname = surname,
            Email = email,
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

    private static Proposal CreateProposal(User user, int daysAgo, string title, string topic, string purpose, string explanation)
    {
        return new Proposal
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            User = user,
            CreatedAt = DateTime.UtcNow.AddDays(daysAgo),
            Title = title,
            Topic = topic,
            Purpose = purpose,
            Explanation = explanation
        };
    }

    private static Evaluation CreateEvaluation(User user, Proposal proposal, string comment, int score, bool isPositive)
    {
        return new Evaluation
        {
            Id = Guid.NewGuid(),
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