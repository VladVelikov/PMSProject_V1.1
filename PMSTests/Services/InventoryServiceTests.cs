using MockQueryable.Moq;
using Moq;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data;
using PMSWeb.ViewModels.InventoryVM;

namespace PMSTests.Services
{
    [TestFixture]
    public class InventoryServiceTests
    {
        private Mock<IRepository<Sparepart, Guid>> _mockSparesRepo;
        private Mock<IRepository<Consumable, Guid>> _mockConsumablesRepo;
        private InventoryService _inventoryService;

        [SetUp]
        public void Setup()
        {
            _mockSparesRepo = new Mock<IRepository<Sparepart, Guid>>();
            _mockConsumablesRepo = new Mock<IRepository<Consumable, Guid>>();

            _inventoryService = new InventoryService(
                _mockSparesRepo.Object,
                _mockConsumablesRepo.Object
            );
        }

        [Test]
        public async Task GetSparesInventoryViewModelAsync_ReturnsCorrectViewModel()
        {
            // Arrange
            var spareParts = new List<Sparepart>
            {
                new Sparepart
                {
                    SparepartId = Guid.NewGuid(),
                    SparepartName = "Spare A",
                    ROB = 10,
                    Units = "pcs",
                    Price = 100,
                    EditedOn = DateTime.Now,
                    IsDeleted = false
                },
                new Sparepart
                {
                    SparepartId = Guid.NewGuid(),
                    SparepartName = "Spare B",
                    ROB = 5,
                    Units = "pcs",
                    Price = 50,
                    EditedOn = DateTime.Now.AddDays(-1),
                    IsDeleted = false
                }
            };

            var mockDbSet = spareParts.AsQueryable().BuildMockDbSet();
            _mockSparesRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _inventoryService.GetSparesInventoryViewModelAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Spare Parts Inventory", result.Name);
            Assert.AreEqual(2, result.Spares.Count);
            Assert.AreEqual("Spare A", result.Spares.First().Name);
        }

        [Test]
        public async Task GetConsumablesInventoryViewModelAsync_ReturnsCorrectViewModel()
        {
            // Arrange
            var consumables = new List<Consumable>
            {
                new Consumable
                {
                    ConsumableId = Guid.NewGuid(),
                    Name = "Consumable A",
                    ROB = 20,
                    Units = "liters",
                    Price = 200,
                    EditedOn = DateTime.Now,
                    IsDeleted = false
                },
                new Consumable
                {
                    ConsumableId = Guid.NewGuid(),
                    Name = "Consumable B",
                    ROB = 15,
                    Units = "liters",
                    Price = 150,
                    EditedOn = DateTime.Now.AddDays(-1),
                    IsDeleted = false
                }
            };

            var mockDbSet = consumables.AsQueryable().BuildMockDbSet();
            _mockConsumablesRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _inventoryService.GetConsumablesInventoryViewModelAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Consumables Inventory", result.Name);
            Assert.AreEqual(2, result.Consumables.Count);
            Assert.AreEqual("Consumable A", result.Consumables.First().Name);
        }

        [Test]
        public async Task UpdateSparesInventoryAsync_UpdatesRepositoryCorrectly()
        {
            // Arrange
            var spareParts = new List<Sparepart>
            {
                new Sparepart
                {
                    SparepartId = Guid.NewGuid(),
                    SparepartName = "Spare A",
                    ROB = 10,
                    Units = "pcs",
                    Price = 100,
                    EditedOn = DateTime.Now,
                    IsDeleted = false
                }
            };

            var sparesInventoryViewModel = new SparesInventoryViewModel
            {
                Spares = new List<InventoryItemViewModel>
                {
                    new InventoryItemViewModel
                    {
                        Id = spareParts.First().SparepartId.ToString(),
                        Name = "Spare A",
                        Used = 5 // New stock value
                    }
                }
            };

            var mockDbSet = spareParts.AsQueryable().BuildMockDbSet();
            _mockSparesRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Corrected Setup for UpdateRange
            _mockSparesRepo
                .Setup(r => r.UpdateRange(It.IsAny<List<Sparepart>>()))
                .ReturnsAsync(true);

            // Act
            var result = await _inventoryService.UpdateSparesInventoryAsync(sparesInventoryViewModel);

            // Assert
            Assert.IsTrue(result);
            _mockSparesRepo.Verify(r => r.UpdateRange(It.Is<List<Sparepart>>(list => list.First().ROB == 5)), Times.Once);
        }

        [Test]
        public async Task UpdateConsumablesInventoryAsync_UpdatesRepositoryCorrectly()
        {
            // Arrange
            var consumables = new List<Consumable>
            {
                new Consumable
                {
                    ConsumableId = Guid.NewGuid(),
                    Name = "Consumable A",
                    ROB = 20,
                    Units = "liters",
                    Price = 200,
                    EditedOn = DateTime.Now,
                    IsDeleted = false
                }
            };

            var consumablesInventoryViewModel = new ConsumablesInventoryViewModel
            {
                Consumables = new List<InventoryItemViewModel>
                {
                    new InventoryItemViewModel
                    {
                        Id = consumables.First().ConsumableId.ToString(),
                        Name = "Consumable A",
                        Used = 10 // New stock value
                    }
                }
            };

            var mockDbSet = consumables.AsQueryable().BuildMockDbSet();
            _mockConsumablesRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Corrected Setup for UpdateRange
            _mockConsumablesRepo
                .Setup(r => r.UpdateRange(It.IsAny<List<Consumable>>()))
                .ReturnsAsync(true);

            // Act
            var result = await _inventoryService.UpdateConsumablesInventoryAsync(consumablesInventoryViewModel);

            // Assert
            Assert.IsTrue(result);
            _mockConsumablesRepo.Verify(r => r.UpdateRange(It.Is<List<Consumable>>(list => list.First().ROB == 10)), Times.Once);
        }
    }
}
