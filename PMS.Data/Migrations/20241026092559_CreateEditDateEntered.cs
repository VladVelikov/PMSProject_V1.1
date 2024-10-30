using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateEditDateEntered : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Suppliers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when created on");

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedOn",
                table: "Suppliers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when last edited");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "SpecificMaintenances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when created on");

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedOn",
                table: "SpecificMaintenances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when last edited");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "RoutineMaintenances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when created on");

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedOn",
                table: "RoutineMaintenances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when last edited");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Manuals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when created on");

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedOn",
                table: "Manuals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when last edited");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Makers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when created on");

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedOn",
                table: "Makers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when last edited");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Equipments",
                type: "datetime2",
                nullable: false,
                comment: "Date when created on",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedOn",
                table: "Equipments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when last edited");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Countries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when created on");

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedOn",
                table: "Countries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when last edited");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Consumables",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when created on");

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedOn",
                table: "Consumables",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when last edited");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Cities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when created on");

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedOn",
                table: "Cities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when last edited");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "EditedOn",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "SpecificMaintenances");

            migrationBuilder.DropColumn(
                name: "EditedOn",
                table: "SpecificMaintenances");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "RoutineMaintenances");

            migrationBuilder.DropColumn(
                name: "EditedOn",
                table: "RoutineMaintenances");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Manuals");

            migrationBuilder.DropColumn(
                name: "EditedOn",
                table: "Manuals");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Makers");

            migrationBuilder.DropColumn(
                name: "EditedOn",
                table: "Makers");

            migrationBuilder.DropColumn(
                name: "EditedOn",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "EditedOn",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Consumables");

            migrationBuilder.DropColumn(
                name: "EditedOn",
                table: "Consumables");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "EditedOn",
                table: "Cities");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Equipments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Date when created on");
        }
    }
}
