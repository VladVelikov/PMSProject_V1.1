using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class JobOrderAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobOrders",
                columns: table => new
                {
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the Job Order."),
                    JobName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The name of JobOrder"),
                    JobDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Standard or modified description of JobOrder"),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The next due date when the job should be completed"),
                    LastDoneDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The last date when this job was done."),
                    Interval = table.Column<int>(type: "int", nullable: false, comment: "Interval betweed LastDone and DueDate."),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Type of maintenance: Routine or Specific"),
                    ResponsiblePosition = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Default of modified Position responsible for the job."),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Unique identifier of the creator of the JobOrder."),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the equipment that will be maintaind."),
                    RoutineMaintenanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the routine maintenance selected if any"),
                    SpecificMaintenanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the specific maintenance selected if any")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOrders", x => x.JobId);
                    table.ForeignKey(
                        name: "FK_JobOrders_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobOrders_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId");
                    table.ForeignKey(
                        name: "FK_JobOrders_RoutineMaintenances_RoutineMaintenanceId",
                        column: x => x.RoutineMaintenanceId,
                        principalTable: "RoutineMaintenances",
                        principalColumn: "RoutMaintId");
                    table.ForeignKey(
                        name: "FK_JobOrders_SpecificMaintenances_SpecificMaintenanceId",
                        column: x => x.SpecificMaintenanceId,
                        principalTable: "SpecificMaintenances",
                        principalColumn: "SpecMaintId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobOrders_CreatorId",
                table: "JobOrders",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOrders_EquipmentId",
                table: "JobOrders",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOrders_RoutineMaintenanceId",
                table: "JobOrders",
                column: "RoutineMaintenanceId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOrders_SpecificMaintenanceId",
                table: "JobOrders",
                column: "SpecificMaintenanceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobOrders");
        }
    }
}
