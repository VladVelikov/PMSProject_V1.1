using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PMSWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class CityCountrySeeded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "CityId", "Name" },
                values: new object[,]
                {
                    { new Guid("0af7364b-871f-48fa-8053-e0657555f479"), "Mestre" },
                    { new Guid("4eb7ea90-edb1-44d0-a45a-80c32aff36cc"), "Shanghai" },
                    { new Guid("6846628f-ecce-440b-bc9b-961967d3fe06"), "Genova" },
                    { new Guid("74e96f3d-cbf8-4fc1-950c-786988c84027"), "Vienna" },
                    { new Guid("8c139e48-1d8b-402c-be1e-58c18e8f2442"), "Sofia" },
                    { new Guid("8e4b968e-50f4-4bc3-a762-e4725123b45a"), "Pusan" },
                    { new Guid("a984224e-c384-49ef-bf49-01f7bd54346f"), "Varna" },
                    { new Guid("b1dc5eb3-3f08-4bb6-a299-3deb427ce966"), "Boston" },
                    { new Guid("bd3f5fbb-f410-4e1e-bf4e-60940407e049"), "Hamburg" },
                    { new Guid("c6682c8e-a7ce-4180-ae5f-295a42387377"), "Paris" }
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "Name" },
                values: new object[,]
                {
                    { new Guid("2a2fe88c-607a-49ef-a2d5-bee263c3f486"), "Germany" },
                    { new Guid("3f810398-5dec-424c-8566-d43ea7dca0fd"), "France" },
                    { new Guid("539d4c47-00a2-404a-af18-4bd8e5d36f57"), "Italy" },
                    { new Guid("54bb1e27-0a42-417f-8932-544e6cc12bc3"), "Britain" },
                    { new Guid("6779276b-9a3e-4b3f-9ace-c1f262882fbd"), "S.Korea" },
                    { new Guid("6aec1895-1471-4c3b-add3-842ac2d4397e"), "China" },
                    { new Guid("8ea49dff-f677-4dc7-b77f-c3279952c931"), "Bulgaria" },
                    { new Guid("a0bd300e-cf55-45aa-b42d-9ff04f3d2255"), "Poland" },
                    { new Guid("b3dce79b-650f-4048-a53d-b2605a5fb593"), "Singapore" },
                    { new Guid("c0713127-efac-4a13-b370-40d642825387"), "Spain" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "CityId",
                keyValue: new Guid("0af7364b-871f-48fa-8053-e0657555f479"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "CityId",
                keyValue: new Guid("4eb7ea90-edb1-44d0-a45a-80c32aff36cc"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "CityId",
                keyValue: new Guid("6846628f-ecce-440b-bc9b-961967d3fe06"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "CityId",
                keyValue: new Guid("74e96f3d-cbf8-4fc1-950c-786988c84027"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "CityId",
                keyValue: new Guid("8c139e48-1d8b-402c-be1e-58c18e8f2442"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "CityId",
                keyValue: new Guid("8e4b968e-50f4-4bc3-a762-e4725123b45a"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "CityId",
                keyValue: new Guid("a984224e-c384-49ef-bf49-01f7bd54346f"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "CityId",
                keyValue: new Guid("b1dc5eb3-3f08-4bb6-a299-3deb427ce966"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "CityId",
                keyValue: new Guid("bd3f5fbb-f410-4e1e-bf4e-60940407e049"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "CityId",
                keyValue: new Guid("c6682c8e-a7ce-4180-ae5f-295a42387377"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("2a2fe88c-607a-49ef-a2d5-bee263c3f486"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("3f810398-5dec-424c-8566-d43ea7dca0fd"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("539d4c47-00a2-404a-af18-4bd8e5d36f57"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("54bb1e27-0a42-417f-8932-544e6cc12bc3"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("6779276b-9a3e-4b3f-9ace-c1f262882fbd"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("6aec1895-1471-4c3b-add3-842ac2d4397e"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("8ea49dff-f677-4dc7-b77f-c3279952c931"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("a0bd300e-cf55-45aa-b42d-9ff04f3d2255"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("b3dce79b-650f-4048-a53d-b2605a5fb593"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("c0713127-efac-4a13-b370-40d642825387"));
        }
    }
}
