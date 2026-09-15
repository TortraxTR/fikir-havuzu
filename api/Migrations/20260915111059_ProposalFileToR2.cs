using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class ProposalFileToR2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "content",
                table: "ProposalFile");

            migrationBuilder.AddColumn<long>(
                name: "size_bytes",
                table: "ProposalFile",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "storage_key",
                table: "ProposalFile",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "size_bytes",
                table: "ProposalFile");

            migrationBuilder.DropColumn(
                name: "storage_key",
                table: "ProposalFile");

            migrationBuilder.AddColumn<byte[]>(
                name: "content",
                table: "ProposalFile",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
