using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class PurchasedIdAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ItemId",
                table: "RequisitionItems",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Unique identifier of the RequisitionItem in the Requisition Item Table",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Unique identifier of the ReqItem");

            migrationBuilder.AddColumn<Guid>(
                name: "PurchasedItemId",
                table: "RequisitionItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Unique identifier of the item, that will be purchased -> Consumable or Spare");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchasedItemId",
                table: "RequisitionItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemId",
                table: "RequisitionItems",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Unique identifier of the ReqItem",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Unique identifier of the RequisitionItem in the Requisition Item Table");
        }
    }
}
