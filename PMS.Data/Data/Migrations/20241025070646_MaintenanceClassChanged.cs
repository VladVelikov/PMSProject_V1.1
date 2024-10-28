using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class MaintenanceClassChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConditionAfter",
                table: "SpecificMaintenances");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "SpecificMaintenances");

            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "SpecificMaintenances");

            migrationBuilder.DropColumn(
                name: "EmployeePosition",
                table: "SpecificMaintenances");

            migrationBuilder.DropColumn(
                name: "Satus",
                table: "SpecificMaintenances");

            migrationBuilder.DropColumn(
                name: "ConditionAfter",
                table: "RoutineMaintenances");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "RoutineMaintenances");

            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "RoutineMaintenances");

            migrationBuilder.DropColumn(
                name: "EmployeePosition",
                table: "RoutineMaintenances");

            migrationBuilder.DropColumn(
                name: "IsPostponed",
                table: "RoutineMaintenances");

            migrationBuilder.DropColumn(
                name: "Satus",
                table: "RoutineMaintenances");

            migrationBuilder.RenameColumn(
                name: "CompletedDate",
                table: "SpecificMaintenances",
                newName: "LastCompletedDate");

            migrationBuilder.RenameColumn(
                name: "CompletedDate",
                table: "RoutineMaintenances",
                newName: "LastCompletedDate");

            migrationBuilder.AddColumn<string>(
                name: "ResponsiblePosition",
                table: "SpecificMaintenances",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                comment: "Position of the person responsible for the maintenance");

            migrationBuilder.AddColumn<string>(
                name: "ResponsiblePosition",
                table: "RoutineMaintenances",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                comment: "Position of the person responsible for the maintenance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponsiblePosition",
                table: "SpecificMaintenances");

            migrationBuilder.DropColumn(
                name: "ResponsiblePosition",
                table: "RoutineMaintenances");

            migrationBuilder.RenameColumn(
                name: "LastCompletedDate",
                table: "SpecificMaintenances",
                newName: "CompletedDate");

            migrationBuilder.RenameColumn(
                name: "LastCompletedDate",
                table: "RoutineMaintenances",
                newName: "CompletedDate");

            migrationBuilder.AddColumn<int>(
                name: "ConditionAfter",
                table: "SpecificMaintenances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Condition of equipment after the job");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "SpecificMaintenances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when maintanance should be done");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "SpecificMaintenances",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                comment: "Name of the employee who did it");

            migrationBuilder.AddColumn<string>(
                name: "EmployeePosition",
                table: "SpecificMaintenances",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                comment: "Position of the employee who did it");

            migrationBuilder.AddColumn<int>(
                name: "Satus",
                table: "SpecificMaintenances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Current status of the maintenance");

            migrationBuilder.AddColumn<int>(
                name: "ConditionAfter",
                table: "RoutineMaintenances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Condition of equipment after the job");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "RoutineMaintenances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when maintanance should be done");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "RoutineMaintenances",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                comment: "Name of the employee who did it");

            migrationBuilder.AddColumn<string>(
                name: "EmployeePosition",
                table: "RoutineMaintenances",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                comment: "Position of the employee who did it");

            migrationBuilder.AddColumn<bool>(
                name: "IsPostponed",
                table: "RoutineMaintenances",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Is it postponed - the maintenance");

            migrationBuilder.AddColumn<int>(
                name: "Satus",
                table: "RoutineMaintenances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Current status of the maintenance");
        }
    }
}
