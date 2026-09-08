using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "User",
                type: "character varying(254)",
                maxLength: 254,
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE "User"
                SET email = 'user-' || replace(id::text, '-', '') || '@example.invalid'
                WHERE email IS NULL;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "User",
                type: "character varying(254)",
                maxLength: 254,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(254)",
                oldMaxLength: 254,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "Kullanıcı_email_key",
                table: "User",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Kullanıcı_email_key",
                table: "User");

            migrationBuilder.DropColumn(
                name: "email",
                table: "User");
        }
    }
}
