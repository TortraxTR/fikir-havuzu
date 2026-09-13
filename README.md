# Fikir Havuzu

Çalışanlardan gelen fikir/önerilerin toplanması, yetkili kullanıcılar tarafından
değerlendirilmesi ve yetki yönetiminin yapılabildiği bir sistem.

- **API:** .NET 8, ASP.NET Core Web API, Entity Framework Core, PostgreSQL
- **Frontend:** React + TypeScript, Vite, MUI
- **Veritabanı şeması:** [docs/database-schema.md](docs/database-schema.md)
- **Açık işler / yol haritası:** [TODO.md](TODO.md)

## Gereksinimler

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) 20+ ve npm
- PostgreSQL (yerel bir kurulum ya da Docker ile bir container)

## Veritabanı

API, `api/appsettings.Development.json` içindeki `ConnectionStrings:FikirHavuzu` bağlantı
dizesini kullanır. Varsayılan değer `localhost:5433` üzerinde, `postgres`/`123456`
kimlik bilgileriyle `FikirHavuzu` adlı bir veritabanı bekler:

```
Host=localhost;Port=5433;Database=FikirHavuzu;Username=postgres;Password=123456
```

Kendi ortamınıza göre bu değeri düzenleyin, ya da Docker ile hızlıca bir Postgres ayağa
kaldırın:

```bash
docker run -d --name fikir-havuzu-pg -e POSTGRES_PASSWORD=123456 -e POSTGRES_DB=FikirHavuzu -p 5433:5432 postgres:16
```

Migration'lar ve seed verisi **elle çalıştırmanıza gerek kalmadan** uygulama her
başladığında otomatik uygulanır (bkz. `api/Program.cs` → `DbSeeder.SeedAsync`). Veritabanı
boşsa örnek kullanıcılar, öneriler ve değerlendirmelerle doldurulur.

## API'yi çalıştırma

```bash
cd api
dotnet restore
dotnet run
```

Varsayılan olarak `http://localhost:5128` üzerinde ayağa kalkar (bkz.
`api/Properties/launchSettings.json`) ve geliştirme ortamında Swagger arayüzü
`http://localhost:5128/swagger` adresinde sunulur.

## Frontend'i çalıştırma

```bash
cd frontend
cp .env.example .env   # gerekirse VITE_API_URL'i düzenleyin
npm install
npm run dev
```

Varsayılan olarak `http://localhost:5173` üzerinde ayağa kalkar. API'nin CORS ayarı bu
adresi zaten beklemektedir (bkz. `api/Program.cs`).

## Giriş yapma

Seed verisindeki tüm kullanıcıların şifresi `123456`'dır (bkz. `api/Seeding/DbSeeder.cs`).
Örneğin, tüm yetkilere sahip kullanıcıyla giriş yapmak için:

| Telefon | Şifre | Yetkiler |
|---|---|---|
| `5551112233` | `123456` | Kullanıcı Yönetimi, Yetki Yönetimi, Öneri Oluşturma, Değerlendirme Oluşturma |

## Proje yapısı

```
api/         ASP.NET Core Web API (Controllers → Services → Repositories)
frontend/    React + Vite tek sayfa uygulaması
docs/        Veritabanı şeması ve diğer dokümantasyon
```
