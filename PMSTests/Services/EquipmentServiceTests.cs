using MockQueryable.Moq;
using Moq;
using PMS.Data.Models;
using PMS.Data.Models.Identity;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data;
using PMSWeb.ViewModels.Equipment;

namespace PMSTests.Services
{
    [TestFixture]
    public class EquipmentServiceTests
    {
        private Mock<IRepository<Equipment, Guid>> _mockEquipmentRepo;
        private Mock<IRepository<Maker, Guid>> _mockMakerRepo;
        private Mock<IRepository<RoutineMaintenance, Guid>> _mockRoutineMaintenanceRepo;
        private Mock<IRepository<Consumable, Guid>> _mockConsumableRepo;
        private Mock<IRepository<RoutineMaintenanceEquipment, Guid[]>> _mockRoutineMaintenanceEquipmentRepo;
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
            _mockConsumableEquipmentRepo = new Mock<IRepository<ConsumableEquipment, Guid[]>>();

            _equipmentService = new EquipmentService(
                _mockEquipmentRepo.Object,
                _mockMakerRepo.Object,
                _mockRoutineMaintenanceRepo.Object,
                _mockConsumableRepo.Object,
                _mockRoutineMaintenanceEquipmentRepo.Object,
                _mockConsumableEquipmentRepo.Object,
                null,
                null,
                null
            );
        }

        #region ConfirmDeleteAsync Tests

        [Test]
        public async Task ConfirmDeleteAsync_WithValidModel_ReturnsTrue()
        {
            // Arrange
            var equipmentId = Guid.NewGuid();
            var equipment = new Equipment { EquipmentId = equipmentId, IsDeleted = false };
            var model = new EquipmentDeleteViewModel { EquipmentId = equipmentId.ToString() };

            var mockDbSet = new List<Equipment> { equipment }.AsQueryable().BuildMockDbSet();
            _mockEquipmentRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);
            _mockEquipmentRepo.Setup(r => r.UpdateAsync(It.IsAny<Equipment>())).ReturnsAsync(true);

            // Act
            var result = await _equipmentService.ConfirmDeleteAsync(model);

            // Assert
            Assert.IsTrue(result);
            _mockEquipmentRepo.Verify(r => r.UpdateAsync(It.Is<Equipment>(e => e.IsDeleted)), Times.Once);
        }

        [Test]
        public async Task ConfirmDeleteAsync_WithInvalidModel_ReturnsFalse()
        {
            // Act
            var result = await _equipmentService.ConfirmDeleteAsync(null);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region GetCreateModelAsync Tests

        [Test]
        public async Task GetCreateModelAsync_ReturnsValidModel()
        {
            // Arrange
            var makers = new List<Maker> { new Maker { MakerId = Guid.NewGuid(), MakerName = "Test Maker" } };
            var routineMaintenances = new List<RoutineMaintenance> { new RoutineMaintenance { RoutMaintId = Guid.NewGuid(), Name = "Test Maintenance" } };
            var consumables = new List<Consumable> { new Consumable { ConsumableId = Guid.NewGuid(), Name = "Test Consumable" } };

            _mockMakerRepo.Setup(r => r.GetAllAsQueryable()).Returns(makers.AsQueryable().BuildMockDbSet().Object);
            _mockRoutineMaintenanceRepo.Setup(r => r.GetAllAsQueryable()).Returns(routineMaintenances.AsQueryable().BuildMockDbSet().Object);
            _mockConsumableRepo.Setup(r => r.GetAllAsQueryable()).Returns(consumables.AsQueryable().BuildMockDbSet().Object);

            // Act
            var result = await _equipmentService.GetCreateModelAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Makers.Count);
            Assert.AreEqual(1, result.RoutineMaintenances.Count);
            Assert.AreEqual(1, result.Consumables.Count);
        }

        #endregion

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

        #region GetListOfViewModelsAsync Tests

        [Test]
        public async Task GetListOfViewModelsAsync_ReturnsSortedEquipmentList()
        {
            // Arrange
            var equipmentList = new List<Equipment>
    {
        new Equipment
        {
            EquipmentId = Guid.NewGuid(),
            Name = "Equipment B",
            Description = "Description B",
            CreatedOn = DateTime.Now.AddDays(-2),
            EditedOn = DateTime.Now.AddDays(-1),
            Creator = new PMSUser { UserName = "UserB" }, // Properly initialized
            Maker = new Maker { MakerName = "Maker B" },  // Properly initialized
            IsDeleted = false
        },
        new Equipment
        {
            EquipmentId = Guid.NewGuid(),
            Name = "Equipment A",
            Description = "Description A",
            CreatedOn = DateTime.Now.AddDays(-3),
            EditedOn = DateTime.Now, // Latest edited
            Creator = new PMSUser { UserName = "UserA" }, // Properly initialized
            Maker = new Maker { MakerName = "Maker A" },  // Properly initialized
            IsDeleted = false
        }
    };

            var mockDbSet = equipmentList.AsQueryable().BuildMockDbSet();
            _mockEquipmentRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _equipmentService.GetListOfViewModelsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Equipment A", result.First().Name); // Sorted by EditedOn descending
            Assert.AreEqual("Maker A", result.First().Maker);    // Correct Maker for the first equipment
            Assert.AreEqual("UserA", result.First().Creator);   // Correct Creator for the first equipment
        }


        [Test]
        public async Task GetListOfViewModelsAsync_WhenNoEquipmentExist_ReturnsEmptyList()
        {
            // Arrange
            var mockDbSet = new List<Equipment>().AsQueryable().BuildMockDbSet();
            _mockEquipmentRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _equipmentService.GetListOfViewModelsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        #endregion
    }
}
