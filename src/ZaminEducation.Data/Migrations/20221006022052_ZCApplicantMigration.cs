using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZaminEducation.Data.Migrations
{
    public partial class ZCApplicantMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZCApplicantDirections",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZCApplicantDirections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ZCApplicants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserRole = table.Column<int>(type: "int", nullable: false),
                    DirectoryId = table.Column<long>(type: "bigint", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccesToUseMedia = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZCApplicants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZCApplicants_ZCApplicantDirections_DirectoryId",
                        column: x => x.DirectoryId,
                        principalTable: "ZCApplicantDirections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZCApplicantsAssets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    FileId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZCApplicantsAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZCApplicantsAssets_Attachments_FileId",
                        column: x => x.FileId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZCApplicantsAssets_ZCApplicants_UserId",
                        column: x => x.UserId,
                        principalTable: "ZCApplicants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ZCApplicants_DirectoryId",
                table: "ZCApplicants",
                column: "DirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ZCApplicantsAssets_FileId",
                table: "ZCApplicantsAssets",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_ZCApplicantsAssets_UserId",
                table: "ZCApplicantsAssets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZCApplicantsAssets");

            migrationBuilder.DropTable(
                name: "ZCApplicants");

            migrationBuilder.DropTable(
                name: "ZCApplicantDirections");
        }
    }
}
