using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class SupplierClassChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Suppliers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                comment: "The phone number of supplier",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldComment: "The name of equipment");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Suppliers",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                comment: "The name of supplier",
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60,
                oldComment: "The name of equipment");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Suppliers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "The email of supplier",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldComment: "The name of equipment");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Suppliers",
                type: "nvarchar(450)",
                nullable: false,
                comment: "Unique Identifier Of The Creator",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldComment: "UniqueIdentifierOf The Creator");

            migrationBuilder.AlterColumn<Guid>(
                name: "CountryId",
                table: "Suppliers",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Unique Identifier Of The Country",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "UniqueIdentifierOf The Country");

            migrationBuilder.AlterColumn<Guid>(
                name: "CityId",
                table: "Suppliers",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Unique Identifier Of The City",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "UniqueIdentifierOf The City");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Suppliers",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: false,
                comment: "The address of supplier",
                oldClrType: typeof(string),
                oldType: "nvarchar(90)",
                oldMaxLength: 90,
                oldComment: "The name of equipment");

            migrationBuilder.AlterColumn<Guid>(
                name: "SupplierId",
                table: "Suppliers",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Unique Identifier Of The Supplier",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "UniqueIdentifierOf The Supplier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Suppliers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                comment: "The name of equipment",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldComment: "The phone number of supplier");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Suppliers",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                comment: "The name of equipment",
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60,
                oldComment: "The name of supplier");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Suppliers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "The name of equipment",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldComment: "The email of supplier");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Suppliers",
                type: "nvarchar(450)",
                nullable: false,
                comment: "UniqueIdentifierOf The Creator",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldComment: "Unique Identifier Of The Creator");

            migrationBuilder.AlterColumn<Guid>(
                name: "CountryId",
                table: "Suppliers",
                type: "uniqueidentifier",
                nullable: false,
                comment: "UniqueIdentifierOf The Country",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Unique Identifier Of The Country");

            migrationBuilder.AlterColumn<Guid>(
                name: "CityId",
                table: "Suppliers",
                type: "uniqueidentifier",
                nullable: false,
                comment: "UniqueIdentifierOf The City",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Unique Identifier Of The City");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Suppliers",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: false,
                comment: "The name of equipment",
                oldClrType: typeof(string),
                oldType: "nvarchar(90)",
                oldMaxLength: 90,
                oldComment: "The address of supplier");

            migrationBuilder.AlterColumn<Guid>(
                name: "SupplierId",
                table: "Suppliers",
                type: "uniqueidentifier",
                nullable: false,
                comment: "UniqueIdentifierOf The Supplier",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Unique Identifier Of The Supplier");
        }
    }
}
