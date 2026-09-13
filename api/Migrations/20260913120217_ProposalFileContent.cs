using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class ProposalFileContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file",
                table: "ProposalFile");

            migrationBuilder.AddColumn<byte[]>(
                name: "content",
                table: "ProposalFile",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "content_type",
                table: "ProposalFile",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "file_name",
                table: "ProposalFile",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "content",
                table: "ProposalFile");

            migrationBuilder.DropColumn(
                name: "content_type",
                table: "ProposalFile");

            migrationBuilder.DropColumn(
                name: "file_name",
                table: "ProposalFile");

            migrationBuilder.AddColumn<string>(
                name: "file",
                table: "ProposalFile",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
