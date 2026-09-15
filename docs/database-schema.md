# Veritabanı Şeması

Bu diyagram `api/FikirHavuzuContext.cs` içindeki EF Core model yapılandırmasından türetilmiştir
ve o dosya ile senkron tutulmalıdır (bir migration eklediğinizde burayı da güncelleyin).
GitHub bu bloğu otomatik olarak görsel bir diyagram şeklinde render eder.

```mermaid
erDiagram
    "User" ||--o{ "Proposal" : "oluşturur"
    "User" ||--o{ "Evaluation" : "değerlendirir"
    "Proposal" ||--o{ "Evaluation" : "değerlendirilir"
    "Proposal" ||--o{ "ProposalFile" : "dokümanları"
    "User" }o--o{ "Permission" : "UserPermission üzerinden"

    "User" {
        uuid id PK
        varchar name
        varchar surname
        varchar_254 email UK
        varchar_15 phone UK
        varchar registration_no UK
        varchar_11 government_ID UK "T.C. Kimlik No, checksum doğrulamalı"
        varchar password_hash "BCrypt"
        boolean is_active
    }

    "Permission" {
        uuid id PK
        varchar code UK "USER_MANAGEMENT vb."
        varchar name UK
    }

    "UserPermission" {
        uuid user_id FK
        uuid permission_id FK
    }

    "Proposal" {
        uuid id PK
        uuid user_id FK
        timestamp created_at
        varchar_128 title
        varchar_20 topic "enum: Urun / Hizmet / Surec"
        varchar_128 purpose
        varchar_8192 explanation
    }

    "ProposalFile" {
        uuid id PK
        uuid proposal_id FK
        varchar_255 file_name
        varchar_255 content_type
        varchar_1024 storage_key
        bigint size_bytes
    }

    "Evaluation" {
        uuid id PK
        uuid user_id FK "değerlendiren"
        uuid proposal_id FK
        varchar comment
        int score
        boolean is_positive
    }
```

## Notlar

- Tüm birincil anahtarlar `uuid` (istemci tarafında, DB'ye ilk `Add` anında üretilir).
- `User.email`, `User.phone`, `User.registration_no`, `User.government_ID` ve
  `Permission.code`/`Permission.name` alanlarında benzersizlik (unique) kısıtı vardır.
- `User` ↔ `Permission` ilişkisi `UserPermission` ara tablosu üzerinden çoktan-çoğa (many-to-many);
  ara tablonun birincil anahtarı `(user_id, permission_id)` bileşik anahtarıdır.
- `Proposal.topic`, C# tarafında `ProposalTopic` enum'udur; veritabanında string olarak saklanır
  (`Urun` / `Hizmet` / `Surec`).
- `ProposalFile.storage_key`, dokümanın Cloudflare R2'deki nesne anahtarını tutar; ham baytlar
  veritabanında değil R2'de saklanır (bkz. `api/Services/Storage/`).
- Tablo ve kısıt adları kasıtlı olarak Türkçe bırakılmıştır (bkz. `FikirHavuzuContext.cs`).
