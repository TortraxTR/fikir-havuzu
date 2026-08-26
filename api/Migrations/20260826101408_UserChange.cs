using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class UserChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "creator_id",
                table: "Proposal",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "creator_id",
                table: "Evaluation",
                newName: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Proposal",
                newName: "creator_id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Evaluation",
                newName: "creator_id");
        }
    }
}
