using MockQueryable.Moq;
using Moq;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data;
using PMSWeb.ViewModels.Manual;

namespace PMSTests.Services
{
    [TestFixture]
    public class ManualServiceTests
    {
        private Mock<IRepository<Manual, Guid>> _mockManualsRepo;
        private Mock<IRepository<Maker, Guid>> _mockMakersRepo;
        private Mock<IRepository<Equipment, Guid>> _mockEquipmentsRepo;
        private ManualService _manualService;

        [SetUp]
        public void Setup()
        {
            _mockManualsRepo = new Mock<IRepository<Manual, Guid>>();
            _mockMakersRepo = new Mock<IRepository<Maker, Guid>>();
            _mockEquipmentsRepo = new Mock<IRepository<Equipment, Guid>>();

            _manualService = new ManualService(
                _mockManualsRepo.Object,
                _mockMakersRepo.Object,
                _mockEquipmentsRepo.Object
            );
        }

        [Test]
        public async Task GetListOfViewModelsAsync_ReturnsCorrectList()
        {
            // Arrange
            var manuals = new List<Manual>
            {
                new Manual
                {
                    ManualId = Guid.NewGuid(),
                    ManualName = "Manual A",
                    Maker = new Maker { MakerName = "Maker A" },
                    Equipment = new Equipment { Name = "Equipment A" },
                    EditedOn = DateTime.Now,
                    IsDeleted = false
                },
                new Manual
                {
                    ManualId = Guid.NewGuid(),
                    ManualName = "Manual B",
                    Maker = new Maker { MakerName = "Maker B" },
                    Equipment = new Equipment { Name = "Equipment B" },
                    EditedOn = DateTime.Now.AddDays(-1),
                    IsDeleted = false
                }
            };

            var mockDbSet = manuals.AsQueryable().BuildMockDbSet();
            _mockManualsRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _manualService.GetListOfViewModelsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Manual A", result.First().ManualName);
        }

        [Test]
        public async Task CreateManualAsync_WithValidData_ReturnsTrue()
        {
            // Arrange
            var createModel = new ManualCreateViewModel
            {
                ManualName = "New Manual",
                MakerId = Guid.NewGuid().ToString(),
                EquipmentId = Guid.NewGuid().ToString(),
                ContentURL = "http://example.com/manual"
            };

            _mockManualsRepo.Setup(r => r.AddAsync(It.IsAny<Manual>())).ReturnsAsync(true);

            // Act
            var result = await _manualService.CreateManualAsync(createModel, "user123");

            // Assert
            Assert.IsTrue(result);
            _mockManualsRepo.Verify(r => r.AddAsync(It.Is<Manual>(manual =>
                manual.ManualName == "New Manual" &&
                manual.CreatorId == "user123" &&
                manual.ContentURL == "http://example.com/manual")), Times.Once);
        }

        [Test]
        public async Task GetCreateViewModelAsync_ReturnsValidModel()
        {
            // Arrange
            var makers = new List<Maker>
            {
                new Maker { MakerId = Guid.NewGuid(), MakerName = "Maker A", IsDeleted = false }
            };
            var equipments = new List<Equipment>
            {
                new Equipment { EquipmentId = Guid.NewGuid(), Name = "Equipment A", IsDeleted = false }
            };

            var mockMakersDbSet = makers.AsQueryable().BuildMockDbSet();
            var mockEquipmentsDbSet = equipments.AsQueryable().BuildMockDbSet();

            _mockMakersRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockMakersDbSet.Object);
            _mockEquipmentsRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockEquipmentsDbSet.Object);

            // Act
            var result = await _manualService.GetCreateViewModelAsync("http://example.com/manual");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("http://example.com/manual", result.ContentURL);
            Assert.AreEqual(1, result.Makers.Count);
            Assert.AreEqual("Maker A", result.Makers.First().Name);
            Assert.AreEqual(1, result.Equipments.Count);
            Assert.AreEqual("Equipment A", result.Equipments.First().Name);
        }

        [Test]
        public async Task GetDetailsAsync_WithValidId_ReturnsDetails()
        {
            // Arrange
            var manualId = Guid.NewGuid();
            var manual = new Manual
            {
                ManualId = manualId,
                ManualName = "Manual A",
                ContentURL = "http://example.com/manual",
                Maker = new Maker { MakerName = "Maker A" },
                Equipment = new Equipment { Name = "Equipment A" },
                IsDeleted = false
            };

            var mockDbSet = new List<Manual> { manual }.AsQueryable().BuildMockDbSet();
            _mockManualsRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _manualService.GetDetailsAsync(manualId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Manual A", result.Name);
            Assert.AreEqual("http://example.com/manual", result.URL);
            Assert.AreEqual("Maker A", result.MakerName);
            Assert.AreEqual("Equipment A", result.EquipmentName);
        }

        [Test]
        public async Task ConfirmDeleteAsync_WithValidId_ReturnsTrue()
        {
            // Arrange
            var manualId = Guid.NewGuid();
            var manual = new Manual { ManualId = manualId, IsDeleted = false };

            _mockManualsRepo.Setup(r => r.GetAllAsQueryable()).Returns(new List<Manual> { manual }.AsQueryable().BuildMockDbSet().Object);
            _mockManualsRepo.Setup(r => r.UpdateAsync(It.IsAny<Manual>())).ReturnsAsync(true);

            var deleteModel = new ManualDeleteViewModel { ManualId = manualId.ToString() };

            // Act
            var result = await _manualService.ConfirmDeleteAsync(deleteModel);

            // Assert
            Assert.IsTrue(result);
            _mockManualsRepo.Verify(r => r.UpdateAsync(It.Is<Manual>(m => m.IsDeleted)), Times.Once);
        }
    }
}
