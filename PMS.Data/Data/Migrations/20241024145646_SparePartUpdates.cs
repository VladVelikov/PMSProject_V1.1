using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class SparePartUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spareparts_RoutineMaintenances_RoutineMaintenanceRoutMaintId",
                table: "Spareparts");

            migrationBuilder.DropForeignKey(
                name: "FK_Spareparts_SpecificMaintenances_SpecificMaintenanceSpecMaintId",
                table: "Spareparts");

            migrationBuilder.DropIndex(
                name: "IX_Spareparts_RoutineMaintenanceRoutMaintId",
                table: "Spareparts");

            migrationBuilder.DropIndex(
                name: "IX_Spareparts_SpecificMaintenanceSpecMaintId",
                table: "Spareparts");

            migrationBuilder.DropColumn(
                name: "RoutineMaintenanceRoutMaintId",
                table: "Spareparts");

            migrationBuilder.DropColumn(
                name: "SpecificMaintenanceSpecMaintId",
                table: "Spareparts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoutineMaintenanceRoutMaintId",
                table: "Spareparts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SpecificMaintenanceSpecMaintId",
                table: "Spareparts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Spareparts_RoutineMaintenanceRoutMaintId",
                table: "Spareparts",
                column: "RoutineMaintenanceRoutMaintId");

            migrationBuilder.CreateIndex(
                name: "IX_Spareparts_SpecificMaintenanceSpecMaintId",
                table: "Spareparts",
                column: "SpecificMaintenanceSpecMaintId");

            migrationBuilder.AddForeignKey(
                name: "FK_Spareparts_RoutineMaintenances_RoutineMaintenanceRoutMaintId",
                table: "Spareparts",
                column: "RoutineMaintenanceRoutMaintId",
                principalTable: "RoutineMaintenances",
                principalColumn: "RoutMaintId");

            migrationBuilder.AddForeignKey(
                name: "FK_Spareparts_SpecificMaintenances_SpecificMaintenanceSpecMaintId",
                table: "Spareparts",
                column: "SpecificMaintenanceSpecMaintId",
                principalTable: "SpecificMaintenances",
                principalColumn: "SpecMaintId");
        }
    }
}
