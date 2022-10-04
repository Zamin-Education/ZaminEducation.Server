using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZaminEducation.Data.Migrations
{
    public partial class ZaminCreativeMigration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAssets_Users_UserId",
                table: "UserAssets");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAssets_ApplicantUsers_UserId",
                table: "UserAssets",
                column: "UserId",
                principalTable: "ApplicantUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAssets_ApplicantUsers_UserId",
                table: "UserAssets");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAssets_Users_UserId",
                table: "UserAssets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
