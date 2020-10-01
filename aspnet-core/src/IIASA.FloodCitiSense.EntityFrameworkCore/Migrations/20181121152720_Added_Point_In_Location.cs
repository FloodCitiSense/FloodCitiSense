using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace IIASA.FloodCitiSense.Migrations
{
    public partial class Added_Point_In_Location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", "'postgis', '', ''");

            migrationBuilder.AddColumn<Point>(
                name: "Point",
                table: "Locations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Point",
                table: "Locations");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:postgis", "'postgis', '', ''");
        }
    }
}
