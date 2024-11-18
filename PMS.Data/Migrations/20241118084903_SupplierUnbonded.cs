using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class SupplierUnbonded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequisitionItems_Suppliers_SupplierId",
                table: "RequisitionItems");

            migrationBuilder.DropIndex(
                name: "IX_RequisitionItems_SupplierId",
                table: "RequisitionItems");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "RequisitionItems");

            migrationBuilder.AddColumn<string>(
                name: "SupplierName",
                table: "RequisitionItems",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Unique identifier of the item's supplier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplierName",
                table: "RequisitionItems");

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "RequisitionItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Unique identifier of the item's supplier");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionItems_SupplierId",
                table: "RequisitionItems",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitionItems_Suppliers_SupplierId",
                table: "RequisitionItems",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierId");
        }
    }
}
