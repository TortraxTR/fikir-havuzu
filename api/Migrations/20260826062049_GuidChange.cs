using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class GuidChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("id", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    surname = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    registration_no = table.Column<string>(type: "text", nullable: false),
                    government_ID = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Kullanıcı_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Proposal",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    topic = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    purpose = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    explanation = table.Column<string>(type: "character varying(8192)", maxLength: 8192, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Fikir_pkey", x => x.id);
                    table.ForeignKey(
                        name: "yaratıcı_id",
                        column: x => x.creator_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserPermission",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    permission_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("KullanıcıYetki_pkey", x => new { x.user_id, x.permission_id });
                    table.ForeignKey(
                        name: "UserPermission_permission_id_fkey",
                        column: x => x.permission_id,
                        principalTable: "Permission",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "UserPermission_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evaluation",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: false),
                    proposal_id = table.Column<Guid>(type: "uuid", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    score = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'', '1', '0', '5', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_positive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Yorum_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Evaluation_creator_id_fkey",
                        column: x => x.creator_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Evaluation_proposal_id_fkey",
                        column: x => x.proposal_id,
                        principalTable: "Proposal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProposalFile",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    proposal_id = table.Column<Guid>(type: "uuid", nullable: false),
                    file = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FikirDosya_pkey", x => x.id);
                    table.ForeignKey(
                        name: "ProposalFile_proposal_id_fkey",
                        column: x => x.proposal_id,
                        principalTable: "Proposal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Değerlendirme_fikir_id",
                table: "Evaluation",
                column: "proposal_id");

            migrationBuilder.CreateIndex(
                name: "IX_Değerlendirme_yaratıcı_id",
                table: "Evaluation",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "YetkiAdı",
                table: "Permission",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fikir_yaratıcı_id",
                table: "Proposal",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_FikirDosya_fikir_id",
                table: "ProposalFile",
                column: "proposal_id");

            migrationBuilder.CreateIndex(
                name: "Kullanıcı_kimlikNo_key",
                table: "User",
                column: "government_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Kullanıcı_sicilNo_key",
                table: "User",
                column: "registration_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KullanıcıYetki_yetki_id",
                table: "UserPermission",
                column: "permission_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Evaluation");

            migrationBuilder.DropTable(
                name: "ProposalFile");

            migrationBuilder.DropTable(
                name: "UserPermission");

            migrationBuilder.DropTable(
                name: "Proposal");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
