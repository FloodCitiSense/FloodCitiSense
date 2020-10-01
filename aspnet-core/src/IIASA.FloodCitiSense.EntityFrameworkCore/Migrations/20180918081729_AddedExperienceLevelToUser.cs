using Microsoft.EntityFrameworkCore.Migrations;

namespace IIASA.FloodCitiSense.Migrations
{
    public partial class AddedExperienceLevelToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExperienceLevel",
                table: "AbpUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExperienceLevel",
                table: "AbpUsers");
        }
    }
}
