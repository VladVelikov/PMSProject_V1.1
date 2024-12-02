using MockQueryable.Moq;
using Moq;
using PMS.Data.Models;
using PMS.Data.Models.Identity;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data;

namespace PMSTests.Services
{
    [TestFixture]
    public class EquipmentServiceTests2
    {
        private Mock<IRepository<Equipment, Guid>> _mockEquipmentRepo;
        private Mock<IRepository<Maker, Guid>> _mockMakerRepo;
        private Mock<IRepository<RoutineMaintenance, Guid>> _mockRoutineMaintenanceRepo;
        private Mock<IRepository<Consumable, Guid>> _mockConsumableRepo;
        private Mock<IRepository<RoutineMaintenanceEquipment, Guid[]>> _mockRoutineMaintenanceEquipmentRepo;
        private Mock<IRepository<SpecificMaintenance, Guid>> _mockSpecificMaintenancesRepo;
        private Mock<IRepository<Sparepart, Guid>> _mockSparePartsRepo;
        private Mock<IRepository<Manual, Guid>> _mockManualRepo;
        private Mock<IRepository<ConsumableEquipment, Guid[]>> _mockConsumableEquipmentRepo;

        private EquipmentService _equipmentService;

        [SetUp]
        public void Setup()
        {
            _mockEquipmentRepo = new Mock<IRepository<Equipment, Guid>>();
            _mockMakerRepo = new Mock<IRepository<Maker, Guid>>();
            _mockRoutineMaintenanceRepo = new Mock<IRepository<RoutineMaintenance, Guid>>();
            _mockConsumableRepo = new Mock<IRepository<Consumable, Guid>>();
            _mockRoutineMaintenanceEquipmentRepo = new Mock<IRepository<RoutineMaintenanceEquipment, Guid[]>>();
            _mockSpecificMaintenancesRepo = new Mock<IRepository<SpecificMaintenance, Guid>>();
            _mockSparePartsRepo = new Mock<IRepository<Sparepart, Guid>>();
            _mockManualRepo = new Mock<IRepository<Manual, Guid>>();
            _mockConsumableEquipmentRepo = new Mock<IRepository<ConsumableEquipment, Guid[]>>();

            _equipmentService = new EquipmentService(
                _mockEquipmentRepo.Object,
                _mockMakerRepo.Object,
                _mockRoutineMaintenanceRepo.Object,
                _mockConsumableRepo.Object,
                _mockRoutineMaintenanceEquipmentRepo.Object,
                _mockConsumableEquipmentRepo.Object,
                _mockSpecificMaintenancesRepo.Object,
                _mockSparePartsRepo.Object,
                _mockManualRepo.Object
            );
        }

        [Test]
        public async Task GetDetailsAsync_WithValidId_ReturnsDetails()
        {
            // Arrange
            var equipmentId = Guid.NewGuid();

            // Mock Equipment repository
            var equipment = new Equipment
            {
                EquipmentId = equipmentId,
                Name = "Test Equipment",
                Description = "Test Description",
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now,
                Creator = new PMSUser { UserName = "Test Creator" },
                Maker = new Maker { MakerName = "Test Maker" },
                IsDeleted = false
            };
            var mockEquipmentDbSet = new List<Equipment> { equipment }.AsQueryable().BuildMockDbSet();
            _mockEquipmentRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockEquipmentDbSet.Object);

            // Mock RoutineMaintenanceEquipment repository
            var routineMaintenanceEquipments = new List<RoutineMaintenanceEquipment>
            {
                new RoutineMaintenanceEquipment
                {
                    RoutineMaintenance = new RoutineMaintenance { Name = "Routine Maintenance 1", IsDeleted = false },
                    EquipmentId = equipmentId
                }
            };
            var mockRoutineMaintenanceEquipmentDbSet = routineMaintenanceEquipments.AsQueryable().BuildMockDbSet();
            _mockRoutineMaintenanceEquipmentRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockRoutineMaintenanceEquipmentDbSet.Object);

            // Mock SpecificMaintenance repository
            var specificMaintenances = new List<SpecificMaintenance>
            {
                new SpecificMaintenance { Name = "Specific Maintenance 1", EquipmentId = equipmentId, IsDeleted = false }
            };
            var mockSpecificMaintenanceDbSet = specificMaintenances.AsQueryable().BuildMockDbSet();
            _mockSpecificMaintenancesRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockSpecificMaintenanceDbSet.Object);

            // Mock SpareParts repository
            var spareParts = new List<Sparepart>
            {
                new Sparepart { SparepartName = "Spare Part 1", EquipmentId = equipmentId, IsDeleted = false }
            };
            var mockSparePartsDbSet = spareParts.AsQueryable().BuildMockDbSet();
            _mockSparePartsRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockSparePartsDbSet.Object);

            // Mock Manuals repository
            var manuals = new List<Manual>
            {
                new Manual { ManualName = "Manual 1", EquipmentId = equipmentId, IsDeleted = false }
            };
            var mockManualsDbSet = manuals.AsQueryable().BuildMockDbSet();
            _mockManualRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockManualsDbSet.Object);

            // Mock ConsumableEquipment repository
            var consumableEquipments = new List<ConsumableEquipment>
            {
                new ConsumableEquipment
                {
                    Consumable = new Consumable { Name = "Consumable 1", IsDeleted = false },
                    EquipmentId = equipmentId
                }
            };
            var mockConsumableEquipmentDbSet = consumableEquipments.AsQueryable().BuildMockDbSet();
            _mockConsumableEquipmentRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockConsumableEquipmentDbSet.Object);

            // Act
            var result = await _equipmentService.GetDetailsAsync(equipmentId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(equipmentId.ToString(), result.EquipmentId);
            Assert.AreEqual("Test Equipment", result.Name);
            Assert.AreEqual("Test Maker", result.Maker);
            Assert.AreEqual("Test Creator", result.Creator);
            Assert.AreEqual(1, result.RoutineMaintenances.Count);
            Assert.AreEqual("Routine Maintenance 1", result.RoutineMaintenances.First());
            Assert.AreEqual(1, result.SpecificMaintenances.Count);
            Assert.AreEqual("Specific Maintenance 1", result.SpecificMaintenances.First());
            Assert.AreEqual(1, result.SpareParts.Count);
            Assert.AreEqual("Spare Part 1", result.SpareParts.First());
            Assert.AreEqual(1, result.Manuals.Count);
            Assert.AreEqual("Manual 1", result.Manuals.First());
            Assert.AreEqual(1, result.Consumables.Count);
            Assert.AreEqual("Consumable 1", result.Consumables.First());
        }

        [Test]
        public async Task GetDetailsAsync_WithInvalidId_ReturnsEmptyModel()
        {
            // Arrange
            var mockDbSet = new List<Equipment>().AsQueryable().BuildMockDbSet();
            _mockEquipmentRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _equipmentService.GetDetailsAsync(Guid.NewGuid().ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Name);
        }

        [Test]
        public async Task GetCreateModelAsync_ReturnsValidModel()
        {
            // Arrange
            var makers = new List<Maker>
            {
                new Maker { MakerId = Guid.NewGuid(), MakerName = "Test Maker" }
            };
            var routineMaintenances = new List<RoutineMaintenance>
            {
                new RoutineMaintenance { RoutMaintId = Guid.NewGuid(), Name = "Test Maintenance" }
            };
            var consumables = new List<Consumable>
            {
                new Consumable { ConsumableId = Guid.NewGuid(), Name = "Test Consumable" }
            };

            _mockMakerRepo.Setup(r => r.GetAllAsQueryable()).Returns(makers.AsQueryable().BuildMockDbSet().Object);
            _mockRoutineMaintenanceRepo.Setup(r => r.GetAllAsQueryable()).Returns(routineMaintenances.AsQueryable().BuildMockDbSet().Object);
            _mockConsumableRepo.Setup(r => r.GetAllAsQueryable()).Returns(consumables.AsQueryable().BuildMockDbSet().Object);

            // Act
            var result = await _equipmentService.GetCreateModelAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Makers.Count);
            Assert.AreEqual("Test Maker", result.Makers.First().Name);
            Assert.AreEqual(1, result.RoutineMaintenances.Count);
            Assert.AreEqual("Test Maintenance", result.RoutineMaintenances.First().Name);
            Assert.AreEqual(1, result.Consumables.Count);
            Assert.AreEqual("Test Consumable", result.Consumables.First().Name);
        }
    }
}
