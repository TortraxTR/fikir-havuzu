using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class PermissionCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "Permission",
                type: "text",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE "Permission"
                SET code = CASE name
                    WHEN 'Kullanıcı Yönetimi' THEN 'USER_MANAGEMENT'
                    WHEN 'Yetki Yönetimi' THEN 'PERMISSION_MANAGEMENT'
                    WHEN 'Öneri Oluşturma' THEN 'PROPOSAL_CREATE'
                    WHEN 'Değerlendirme Oluşturma' THEN 'EVALUATION_CREATE'
                    WHEN 'Admin' THEN 'USER_MANAGEMENT'
                    WHEN 'Reviewer' THEN 'EVALUATION_CREATE'
                    WHEN 'Proposer' THEN 'PROPOSAL_CREATE'
                    WHEN 'DepartmentManager' THEN 'PERMISSION_MANAGEMENT'
                    ELSE 'LEGACY_' || replace(id::text, '-', '')
                END
                WHERE code IS NULL;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "Permission",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "Permission_code_key",
                table: "Permission",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Permission_code_key",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "code",
                table: "Permission");
        }
    }
}
