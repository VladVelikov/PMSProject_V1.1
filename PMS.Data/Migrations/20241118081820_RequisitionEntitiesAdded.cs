using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class RequisitionEntitiesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Consumables",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "The name of the consumable",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "The name of the consumable");

            migrationBuilder.CreateTable(
                name: "Requisitions",
                columns: table => new
                {
                    RequisitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the requisition"),
                    RequisitionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The name of the requisition"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date when requisition is created"),
                    RequisitionType = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The type of the requisition Consumable or Spare"),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Unique identifier of the user who created the requisition"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TotalCost = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false, comment: "The total cost of all items in the requisition")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requisitions", x => x.RequisitionId);
                    table.ForeignKey(
                        name: "FK_Requisitions_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequisitionItems",
                columns: table => new
                {
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the ReqItem"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Name of the req item"),
                    OrderedAmount = table.Column<double>(type: "float", nullable: false, comment: "The Amount To Order"),
                    Units = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Measuring units if the req item"),
                    Price = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false, comment: "Price of the requisition item"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Type of the requisition item = Consumable or Spare"),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the item's supplier"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date when item was added to the requisition"),
                    RequisitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Uniquer identifier of the requisition whre item is placed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitionItems", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_RequisitionItems_Requisitions_RequisitionId",
                        column: x => x.RequisitionId,
                        principalTable: "Requisitions",
                        principalColumn: "RequisitionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequisitionItems_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionItems_RequisitionId",
                table: "RequisitionItems",
                column: "RequisitionId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionItems_SupplierId",
                table: "RequisitionItems",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitions_CreatorId",
                table: "Requisitions",
                column: "CreatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequisitionItems");

            migrationBuilder.DropTable(
                name: "Requisitions");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Consumables",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "The name of the consumable",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldComment: "The name of the consumable");
        }
    }
}
