using Microsoft.EntityFrameworkCore.Migrations;

namespace IIASA.FloodCitiSense.Migrations
{
    public partial class sensorReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_AbpUserAccounts_UserAccountId",
                table: "Sensors");

            migrationBuilder.RenameColumn(
                name: "UserAccountId",
                table: "Sensors",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Sensors_UserAccountId",
                table: "Sensors",
                newName: "IX_Sensors_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_AbpUsers_UserId",
                table: "Sensors",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_AbpUsers_UserId",
                table: "Sensors");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Sensors",
                newName: "UserAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Sensors_UserId",
                table: "Sensors",
                newName: "IX_Sensors_UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_AbpUserAccounts_UserAccountId",
                table: "Sensors",
                column: "UserAccountId",
                principalTable: "AbpUserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
