using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IIASA.FloodCitiSense.Migrations
{
    public partial class addedSensor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Incidents_IncidentId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Incidents_IncidentId",
                table: "Pictures");

            migrationBuilder.AlterColumn<int>(
                name: "IncidentId",
                table: "Pictures",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "SensorId",
                table: "Pictures",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IncidentId",
                table: "Locations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "SensorId",
                table: "Locations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    UserAccountId = table.Column<long>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensors_AbpUserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "AbpUserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_SensorId",
                table: "Pictures",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_SensorId",
                table: "Locations",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_TenantId",
                table: "Sensors",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_UserAccountId",
                table: "Sensors",
                column: "UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Incidents_IncidentId",
                table: "Locations",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Sensors_SensorId",
                table: "Locations",
                column: "SensorId",
                principalTable: "Sensors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Incidents_IncidentId",
                table: "Pictures",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Sensors_SensorId",
                table: "Pictures",
                column: "SensorId",
                principalTable: "Sensors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Incidents_IncidentId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Sensors_SensorId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Incidents_IncidentId",
                table: "Pictures");

            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Sensors_SensorId",
                table: "Pictures");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_SensorId",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Locations_SensorId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "SensorId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "SensorId",
                table: "Locations");

            migrationBuilder.AlterColumn<int>(
                name: "IncidentId",
                table: "Pictures",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IncidentId",
                table: "Locations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Incidents_IncidentId",
                table: "Locations",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Incidents_IncidentId",
                table: "Pictures",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
