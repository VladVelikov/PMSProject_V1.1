using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class SparePartReworked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Manuals_Spareparts_SparepartId",
                table: "Manuals");

            migrationBuilder.DropIndex(
                name: "IX_Manuals_SparepartId",
                table: "Manuals");

            migrationBuilder.DropColumn(
                name: "SparepartId",
                table: "Manuals");

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Spareparts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Spareparts");

            migrationBuilder.AddColumn<Guid>(
                name: "SparepartId",
                table: "Manuals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Unique identifier of the spare part");

            migrationBuilder.CreateIndex(
                name: "IX_Manuals_SparepartId",
                table: "Manuals",
                column: "SparepartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Manuals_Spareparts_SparepartId",
                table: "Manuals",
                column: "SparepartId",
                principalTable: "Spareparts",
                principalColumn: "SparepartId");
        }
    }
}
