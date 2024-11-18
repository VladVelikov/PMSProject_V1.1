using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class BudgetAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Requisitions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Budget",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier of the budget record.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date When Last Time The Budget Was Increased or Decreased"),
                    Ballance = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false, comment: "Remaining funds in budget.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budget", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Budget");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Requisitions");
        }
    }
}
