using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class MinorBugFixedForSpareParts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Spareparts",
                type: "bit",
                nullable: false,
                comment: "Soft delete for spare part",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Soft delete fpr spare part");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Spareparts",
                type: "bit",
                nullable: false,
                comment: "Soft delete fpr spare part",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Soft delete for spare part");
        }
    }
}
