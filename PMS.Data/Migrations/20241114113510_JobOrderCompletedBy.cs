using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class JobOrderCompletedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "JobDescription",
                table: "JobOrders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                comment: "Standard or modified description of JobOrder",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComment: "Standard or modified description of JobOrder");

            migrationBuilder.AddColumn<string>(
                name: "CompletedBy",
                table: "JobOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedBy",
                table: "JobOrders");

            migrationBuilder.AlterColumn<string>(
                name: "JobDescription",
                table: "JobOrders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "Standard or modified description of JobOrder",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldComment: "Standard or modified description of JobOrder");
        }
    }
}
