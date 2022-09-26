using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZaminEducation.Data.Migrations
{
    public partial class QuizResultAttachmentMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AttachmentId",
                table: "QuizResults",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuizResults_AttachmentId",
                table: "QuizResults",
                column: "AttachmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizResults_Attachments_AttachmentId",
                table: "QuizResults",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizResults_Attachments_AttachmentId",
                table: "QuizResults");

            migrationBuilder.DropIndex(
                name: "IX_QuizResults_AttachmentId",
                table: "QuizResults");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "QuizResults");
        }
    }
}
