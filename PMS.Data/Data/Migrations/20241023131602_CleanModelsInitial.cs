using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class CleanModelsInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullUserName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PositionId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                });

            migrationBuilder.CreateTable(
                name: "Consumables",
                columns: table => new
                {
                    ConsumableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the Consumable"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The name of the consumable"),
                    Units = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "The measuring unit of the consumable"),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true, comment: "Description of the consumable"),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Unique identifier of the Creator"),
                    Price = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false, comment: "The price of the consumable per unit"),
                    ROB = table.Column<double>(type: "float", nullable: false, comment: "The remaining quantity on stock"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumables", x => x.ConsumableId);
                    table.ForeignKey(
                        name: "FK_Consumables_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Makers",
                columns: table => new
                {
                    MakerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of Maker"),
                    MakerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Name of the maker"),
                    Description = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true, comment: "Description of the maker"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "The E-mail of the maker"),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "Phone number of the maker"),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Id of the creator of this Maker entry"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Soft delete implemented")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Makers", x => x.MakerId);
                    table.ForeignKey(
                        name: "FK_Makers_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoutineMaintenances",
                columns: table => new
                {
                    RoutMaintId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the RoutineMaintenance"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Name of the maintanance"),
                    Description = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true, comment: "Description of the maintenance"),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date when maintanance should be done"),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date when maintanance is completed"),
                    Interval = table.Column<int>(type: "int", nullable: false, comment: "Interval to do the maintanance"),
                    Satus = table.Column<int>(type: "int", nullable: false, comment: "Current status of the maintenance"),
                    ConditionAfter = table.Column<int>(type: "int", nullable: false, comment: "Condition of equipment after the job"),
                    IsPostponed = table.Column<bool>(type: "bit", nullable: false, comment: "Is it postponed - the maintenance"),
                    EmployeePosition = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Position of the employee who did it"),
                    EmployeeName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Name of the employee who did it"),
                    CReatorId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Unique identifier of the creator of the maintenance"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutineMaintenances", x => x.RoutMaintId);
                    table.ForeignKey(
                        name: "FK_RoutineMaintenances_AspNetUsers_CReatorId",
                        column: x => x.CReatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "UniqueIdentifierOf The Supplier"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false, comment: "The name of equipment"),
                    Address = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: false, comment: "The name of equipment"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "The name of equipment"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "The name of equipment"),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "UniqueIdentifierOf The City"),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "UniqueIdentifierOf The Country"),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "UniqueIdentifierOf The Creator"),
                    IsDleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
                    table.ForeignKey(
                        name: "FK_Suppliers_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Suppliers_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Suppliers_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the Equipment"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The name of equipment"),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Description of equipment"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Unique identifier of the Creator"),
                    MakerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the Maker"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.EquipmentId);
                    table.ForeignKey(
                        name: "FK_Equipments_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Equipments_Makers_MakerId",
                        column: x => x.MakerId,
                        principalTable: "Makers",
                        principalColumn: "MakerId");
                });

            migrationBuilder.CreateTable(
                name: "ConsumablesSuppliers",
                columns: table => new
                {
                    ConsumableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumablesSuppliers", x => new { x.ConsumableId, x.SupplierId });
                    table.ForeignKey(
                        name: "FK_ConsumablesSuppliers_Consumables_ConsumableId",
                        column: x => x.ConsumableId,
                        principalTable: "Consumables",
                        principalColumn: "ConsumableId");
                    table.ForeignKey(
                        name: "FK_ConsumablesSuppliers_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                });

            migrationBuilder.CreateTable(
                name: "ConsumablesEquipments",
                columns: table => new
                {
                    ConsumableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumablesEquipments", x => new { x.ConsumableId, x.EquipmentId });
                    table.ForeignKey(
                        name: "FK_ConsumablesEquipments_Consumables_ConsumableId",
                        column: x => x.ConsumableId,
                        principalTable: "Consumables",
                        principalColumn: "ConsumableId");
                    table.ForeignKey(
                        name: "FK_ConsumablesEquipments_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId");
                });

            migrationBuilder.CreateTable(
                name: "RoutineMaintenancesEquipments",
                columns: table => new
                {
                    RoutineMaintenanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutineMaintenancesEquipments", x => new { x.RoutineMaintenanceId, x.EquipmentId });
                    table.ForeignKey(
                        name: "FK_RoutineMaintenancesEquipments_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId");
                    table.ForeignKey(
                        name: "FK_RoutineMaintenancesEquipments_RoutineMaintenances_RoutineMaintenanceId",
                        column: x => x.RoutineMaintenanceId,
                        principalTable: "RoutineMaintenances",
                        principalColumn: "RoutMaintId");
                });

            migrationBuilder.CreateTable(
                name: "SpecificMaintenances",
                columns: table => new
                {
                    SpecMaintId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the RoutineMaintenance"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Name of the maintanance"),
                    Description = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true, comment: "Description of the maintenance"),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date when maintanance should be done"),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date when maintanance is completed"),
                    Interval = table.Column<int>(type: "int", nullable: false, comment: "Interval to do the maintanance"),
                    Satus = table.Column<int>(type: "int", nullable: false, comment: "Current status of the maintenance"),
                    ConditionAfter = table.Column<int>(type: "int", nullable: false, comment: "Condition of equipment after the job"),
                    IsPostponed = table.Column<bool>(type: "bit", nullable: false, comment: "Is it postponed - the maintenance"),
                    EmployeePosition = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Position of the employee who did it"),
                    EmployeeName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Name of the employee who did it"),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the equipment maintained"),
                    CReatorId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Unique identifier of the creator of the maintenance"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificMaintenances", x => x.SpecMaintId);
                    table.ForeignKey(
                        name: "FK_SpecificMaintenances_AspNetUsers_CReatorId",
                        column: x => x.CReatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecificMaintenances_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId");
                });

            migrationBuilder.CreateTable(
                name: "Spareparts",
                columns: table => new
                {
                    SparepartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "UniqueIdentifier of the spare part"),
                    SparepartName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "The name of the spare part"),
                    Description = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: false, comment: "Description of the spare part"),
                    ROB = table.Column<double>(type: "float", nullable: false, comment: "Remaining stock"),
                    Price = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false, comment: "The price for one unit of the spare"),
                    Units = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Teh measuring units"),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the related equipment"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date when spare created on"),
                    EditedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date when spare last edited"),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Unique  identifier of the creator of the spare"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Soft delete fpr spare part"),
                    RoutineMaintenanceRoutMaintId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SpecificMaintenanceSpecMaintId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spareparts", x => x.SparepartId);
                    table.ForeignKey(
                        name: "FK_Spareparts_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Spareparts_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId");
                    table.ForeignKey(
                        name: "FK_Spareparts_RoutineMaintenances_RoutineMaintenanceRoutMaintId",
                        column: x => x.RoutineMaintenanceRoutMaintId,
                        principalTable: "RoutineMaintenances",
                        principalColumn: "RoutMaintId");
                    table.ForeignKey(
                        name: "FK_Spareparts_SpecificMaintenances_SpecificMaintenanceSpecMaintId",
                        column: x => x.SpecificMaintenanceSpecMaintId,
                        principalTable: "SpecificMaintenances",
                        principalColumn: "SpecMaintId");
                });

            migrationBuilder.CreateTable(
                name: "Manuals",
                columns: table => new
                {
                    ManualId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the manual"),
                    ManualName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Name of the manual"),
                    MakerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the maker"),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the equipment"),
                    SparepartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the spare part"),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Unique identifier of the Creator"),
                    ContentURL = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true, comment: "URL to file with content of the manual"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Soft delete implemented")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manuals", x => x.ManualId);
                    table.ForeignKey(
                        name: "FK_Manuals_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Manuals_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId");
                    table.ForeignKey(
                        name: "FK_Manuals_Makers_MakerId",
                        column: x => x.MakerId,
                        principalTable: "Makers",
                        principalColumn: "MakerId");
                    table.ForeignKey(
                        name: "FK_Manuals_Spareparts_SparepartId",
                        column: x => x.SparepartId,
                        principalTable: "Spareparts",
                        principalColumn: "SparepartId");
                });

            migrationBuilder.CreateTable(
                name: "SparepartsSuppliers",
                columns: table => new
                {
                    SparepartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SparepartsSuppliers", x => new { x.SupplierId, x.SparepartId });
                    table.ForeignKey(
                        name: "FK_SparepartsSuppliers_Spareparts_SparepartId",
                        column: x => x.SparepartId,
                        principalTable: "Spareparts",
                        principalColumn: "SparepartId");
                    table.ForeignKey(
                        name: "FK_SparepartsSuppliers_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consumables_CreatorId",
                table: "Consumables",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumablesEquipments_EquipmentId",
                table: "ConsumablesEquipments",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumablesSuppliers_SupplierId",
                table: "ConsumablesSuppliers",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_CreatorId",
                table: "Equipments",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_MakerId",
                table: "Equipments",
                column: "MakerId");

            migrationBuilder.CreateIndex(
                name: "IX_Makers_CreatorId",
                table: "Makers",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Manuals_CreatorId",
                table: "Manuals",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Manuals_EquipmentId",
                table: "Manuals",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Manuals_MakerId",
                table: "Manuals",
                column: "MakerId");

            migrationBuilder.CreateIndex(
                name: "IX_Manuals_SparepartId",
                table: "Manuals",
                column: "SparepartId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutineMaintenances_CReatorId",
                table: "RoutineMaintenances",
                column: "CReatorId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutineMaintenancesEquipments_EquipmentId",
                table: "RoutineMaintenancesEquipments",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Spareparts_CreatorId",
                table: "Spareparts",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Spareparts_EquipmentId",
                table: "Spareparts",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Spareparts_RoutineMaintenanceRoutMaintId",
                table: "Spareparts",
                column: "RoutineMaintenanceRoutMaintId");

            migrationBuilder.CreateIndex(
                name: "IX_Spareparts_SpecificMaintenanceSpecMaintId",
                table: "Spareparts",
                column: "SpecificMaintenanceSpecMaintId");

            migrationBuilder.CreateIndex(
                name: "IX_SparepartsSuppliers_SparepartId",
                table: "SparepartsSuppliers",
                column: "SparepartId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificMaintenances_CReatorId",
                table: "SpecificMaintenances",
                column: "CReatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificMaintenances_EquipmentId",
                table: "SpecificMaintenances",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CityId",
                table: "Suppliers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CountryId",
                table: "Suppliers",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CreatorId",
                table: "Suppliers",
                column: "CreatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumablesEquipments");

            migrationBuilder.DropTable(
                name: "ConsumablesSuppliers");

            migrationBuilder.DropTable(
                name: "Manuals");

            migrationBuilder.DropTable(
                name: "RoutineMaintenancesEquipments");

            migrationBuilder.DropTable(
                name: "SparepartsSuppliers");

            migrationBuilder.DropTable(
                name: "Consumables");

            migrationBuilder.DropTable(
                name: "Spareparts");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "RoutineMaintenances");

            migrationBuilder.DropTable(
                name: "SpecificMaintenances");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "Makers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullUserName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "AspNetUsers");
        }
    }
}
