using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class JobOrderRemakeWithMaintenanceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOrders_RoutineMaintenances_RoutineMaintenanceId",
                table: "JobOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOrders_SpecificMaintenances_SpecificMaintenanceId",
                table: "JobOrders");

            migrationBuilder.DropIndex(
                name: "IX_JobOrders_RoutineMaintenanceId",
                table: "JobOrders");

            migrationBuilder.DropIndex(
                name: "IX_JobOrders_SpecificMaintenanceId",
                table: "JobOrders");

            migrationBuilder.DropColumn(
                name: "RoutineMaintenanceId",
                table: "JobOrders");

            migrationBuilder.DropColumn(
                name: "SpecificMaintenanceId",
                table: "JobOrders");

            migrationBuilder.AddColumn<Guid>(
                name: "MaintenanceId",
                table: "JobOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Unique identifier for the current Type Of Maintenance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaintenanceId",
                table: "JobOrders");

            migrationBuilder.AddColumn<Guid>(
                name: "RoutineMaintenanceId",
                table: "JobOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Unique identifier of the routine maintenance selected if any");

            migrationBuilder.AddColumn<Guid>(
                name: "SpecificMaintenanceId",
                table: "JobOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Unique identifier of the specific maintenance selected if any");

            migrationBuilder.CreateIndex(
                name: "IX_JobOrders_RoutineMaintenanceId",
                table: "JobOrders",
                column: "RoutineMaintenanceId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOrders_SpecificMaintenanceId",
                table: "JobOrders",
                column: "SpecificMaintenanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOrders_RoutineMaintenances_RoutineMaintenanceId",
                table: "JobOrders",
                column: "RoutineMaintenanceId",
                principalTable: "RoutineMaintenances",
                principalColumn: "RoutMaintId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOrders_SpecificMaintenances_SpecificMaintenanceId",
                table: "JobOrders",
                column: "SpecificMaintenanceId",
                principalTable: "SpecificMaintenances",
                principalColumn: "SpecMaintId");
        }
    }
}
