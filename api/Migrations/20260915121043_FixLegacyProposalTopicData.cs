using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class FixLegacyProposalTopicData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The 20260913123622_ProposalTopicEnum migration narrowed "topic" to the
            // ProposalTopic enum (Urun/Hizmet/Surec) but never converted rows written
            // while the column was still free text, which crashes every read via
            // StringEnumConverter. Map the known pre-enum labels (see the DbSeeder.cs
            // history) to their current equivalents, and fall back anything else
            // unrecognized to Surec so a stray legacy value can never crash a query again.
            migrationBuilder.Sql(
                """
                UPDATE "Proposal" SET topic = CASE topic
                    WHEN 'Sürdürülebilirlik' THEN 'Urun'
                    WHEN 'Çevre' THEN 'Urun'
                    WHEN 'Kurum İçi Gelişim' THEN 'Hizmet'
                    WHEN 'İletişim' THEN 'Hizmet'
                    WHEN 'Operasyon' THEN 'Hizmet'
                    WHEN 'Dijital Dönüşüm' THEN 'Surec'
                    WHEN 'Çalışan Deneyimi' THEN 'Surec'
                    WHEN 'İnsan Kaynakları' THEN 'Surec'
                    ELSE 'Surec'
                END
                WHERE topic NOT IN ('Urun', 'Hizmet', 'Surec');
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Irreversible: the original free-text topic labels are not recoverable.
        }
    }
}
