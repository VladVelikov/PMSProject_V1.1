using MockQueryable.Moq;
using Moq;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data;
using PMSWeb.ViewModels.Maker;

namespace PMSTests.Services
{
    [TestFixture]
    public class MakerServiceTests
    {
        private Mock<IRepository<Maker, Guid>> _mockMakersRepo;
        private Mock<IRepository<Manual, Guid>> _mockManualsRepo;
        private Mock<IRepository<Equipment, Guid>> _mockEquipmentRepo;
        private MakerService _makerService;

        [SetUp]
        public void Setup()
        {
            _mockMakersRepo = new Mock<IRepository<Maker, Guid>>();
            _mockManualsRepo = new Mock<IRepository<Manual, Guid>>();
            _mockEquipmentRepo = new Mock<IRepository<Equipment, Guid>>();

            _makerService = new MakerService(
                _mockMakersRepo.Object,
                _mockManualsRepo.Object,
                _mockEquipmentRepo.Object
            );
        }

        [Test]
        public async Task CreateMakerAsync_WithValidData_ReturnsTrue()
        {
            // Arrange
            var makerModel = new MakerCreateViewModel
            {
                MakerName = "Maker A",
                Description = "Description A",
                Email = "maker@example.com",
                Phone = "123456789"
            };

            _mockMakersRepo.Setup(r => r.AddAsync(It.IsAny<Maker>())).ReturnsAsync(true);

            // Act
            var result = await _makerService.CreateMakerAsync(makerModel, "user123");

            // Assert
            Assert.IsTrue(result);
            _mockMakersRepo.Verify(r => r.AddAsync(It.Is<Maker>(maker =>
                maker.MakerName == "Maker A" &&
                maker.CreatorId == "user123")), Times.Once);
        }

        [Test]
        public async Task GetListOfViewModelsAsync_ReturnsCorrectList()
        {
            // Arrange
            var makers = new List<Maker>
            {
                new Maker
                {
                    MakerId = Guid.NewGuid(),
                    MakerName = "Maker A",
                    Description = "Description A",
                    Email = "maker@example.com",
                    Phone = "123456789",
                    EditedOn = DateTime.Now,
                    IsDeleted = false
                },
                new Maker
                {
                    MakerId = Guid.NewGuid(),
                    MakerName = "Maker B",
                    Description = "Description B",
                    Email = "makerB@example.com",
                    Phone = "987654321",
                    EditedOn = DateTime.Now.AddDays(-1),
                    IsDeleted = false
                }
            };

            var mockDbSet = makers.AsQueryable().BuildMockDbSet();
            _mockMakersRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _makerService.GetListOfViewModelsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Maker A", result.First().Name);
        }

        [Test]
        public async Task GetDetailsAsync_WithValidId_ReturnsDetails()
        {
            // Arrange
            var makerId = Guid.NewGuid();
            var maker = new Maker
            {
                MakerId = makerId,
                MakerName = "Maker A",
                Description = "Description A",
                Email = "maker@example.com",
                Phone = "123456789",
                CreatedOn = DateTime.Now.AddDays(-5),
                EditedOn = DateTime.Now,
                IsDeleted = false
            };

            var manuals = new List<Manual>
            {
                new Manual { ManualName = "Manual A", MakerId = makerId, IsDeleted = false },
                new Manual { ManualName = "Manual B", MakerId = makerId, IsDeleted = false }
            };

            var equipments = new List<Equipment>
            {
                new Equipment { Name = "Equipment A", MakerId = makerId, IsDeleted = false },
                new Equipment { Name = "Equipment B", MakerId = makerId, IsDeleted = false }
            };

            var mockMakerDbSet = new List<Maker> { maker }.AsQueryable().BuildMockDbSet();
            var mockManualDbSet = manuals.AsQueryable().BuildMockDbSet();
            var mockEquipmentDbSet = equipments.AsQueryable().BuildMockDbSet();

            _mockMakersRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockMakerDbSet.Object);
            _mockManualsRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockManualDbSet.Object);
            _mockEquipmentRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockEquipmentDbSet.Object);

            // Act
            var result = await _makerService.GetDetailsAsync(makerId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Maker A", result.Name);
            Assert.AreEqual(2, result.ManualsList.Count);
            Assert.AreEqual(2, result.EquipmentsList.Count);
        }

        [Test]
        public async Task SaveItemToEditAsync_WithValidData_ReturnsTrue()
        {
            // Arrange
            var makerId = Guid.NewGuid();
            var maker = new Maker
            {
                MakerId = makerId,
                MakerName = "Maker A",
                Description = "Description A",
                Email = "maker@example.com",
                Phone = "123456789",
                IsDeleted = false
            };

            var editModel = new MakerEditViewModel
            {
                MakerId = makerId.ToString(),
                MakerName = "Maker A Updated",
                Description = "Updated Description",
                Email = "updated@example.com",
                Phone = "987654321"
            };

            var mockDbSet = new List<Maker> { maker }.AsQueryable().BuildMockDbSet();
            _mockMakersRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);
            _mockMakersRepo.Setup(r => r.UpdateAsync(It.IsAny<Maker>())).ReturnsAsync(true);

            // Act
            var result = await _makerService.SaveItemToEditAsync(editModel, "user123");

            // Assert
            Assert.IsTrue(result);
            _mockMakersRepo.Verify(r => r.UpdateAsync(It.Is<Maker>(m =>
                m.MakerName == "Maker A Updated" &&
                m.Email == "updated@example.com")), Times.Once);
        }

        [Test]
        public async Task ConfirmDeleteAsync_WithValidId_ReturnsTrue()
        {
            // Arrange
            var makerId = Guid.NewGuid();
            var maker = new Maker { MakerId = makerId, IsDeleted = false };

            _mockMakersRepo.Setup(r => r.GetAllAsQueryable()).Returns(new List<Maker> { maker }.AsQueryable().BuildMockDbSet().Object);
            _mockMakersRepo.Setup(r => r.UpdateAsync(It.IsAny<Maker>())).ReturnsAsync(true);

            // Act
            var result = await _makerService.ConfirmDeleteAsync(new MakerDeleteViewModel { MakerId = makerId.ToString() });

            // Assert
            Assert.IsTrue(result);
            _mockMakersRepo.Verify(r => r.UpdateAsync(It.Is<Maker>(m => m.IsDeleted)), Times.Once);
        }
    }
}
