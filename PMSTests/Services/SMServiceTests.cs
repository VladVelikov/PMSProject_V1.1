using MockQueryable.Moq;
using Moq;
using PMS.Data.Models;
using PMS.Data.Models.Identity;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data;
using PMSWeb.ViewModels.SM;

namespace PMSTests.Services
{
    [TestFixture]
    public class SMServiceTests
    {
        private Mock<IRepository<SpecificMaintenance, Guid>> _mockSMRepo;
        private Mock<IRepository<Equipment, Guid>> _mockEquipmentRepo;
        private SMService _smService;

        [SetUp]
        public void Setup()
        {
            _mockSMRepo = new Mock<IRepository<SpecificMaintenance, Guid>>();
            _mockEquipmentRepo = new Mock<IRepository<Equipment, Guid>>();

            _smService = new SMService(
                _mockSMRepo.Object,
                _mockEquipmentRepo.Object
            );
        }

        [Test]
        public async Task GetListOfViewModelsAsync_ReturnsCorrectList()
        {
            // Arrange
            var specificMaintenances = new List<SpecificMaintenance>
            {
                new SpecificMaintenance
                {
                    SpecMaintId = Guid.NewGuid(),
                    Name = "Maintenance A",
                    Description = "Description A",
                    Equipment = new Equipment { Name = "Equipment A" },
                    LastCompletedDate = DateTime.Now,
                    Interval = 30,
                    ResponsiblePosition = "Technician",
                    EditedOn = DateTime.Now,
                    IsDeleted = false
                },
                new SpecificMaintenance
                {
                    SpecMaintId = Guid.NewGuid(),
                    Name = "Maintenance B",
                    Description = "Description B",
                    Equipment = new Equipment { Name = "Equipment B" },
                    LastCompletedDate = DateTime.Now.AddDays(-1),
                    Interval = 60,
                    ResponsiblePosition = "Engineer",
                    EditedOn = DateTime.Now.AddDays(-1),
                    IsDeleted = false
                }
            };

            var mockDbSet = specificMaintenances.AsQueryable().BuildMockDbSet();
            _mockSMRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _smService.GetListOfViewModelsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Maintenance A", result.First().Name);
        }

        [Test]
        public async Task GetItemForCreateAsync_ReturnsViewModelWithEquipments()
        {
            // Arrange
            var equipments = new List<Equipment>
            {
                new Equipment { EquipmentId = Guid.NewGuid(), Name = "Equipment A", IsDeleted = false },
                new Equipment { EquipmentId = Guid.NewGuid(), Name = "Equipment B", IsDeleted = false }
            };

            var mockDbSet = equipments.AsQueryable().BuildMockDbSet();
            _mockEquipmentRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _smService.GetItemForCreateAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Equipments.Count);
            Assert.AreEqual("Equipment A", result.Equipments.First().Name);
        }

        [Test]
        public async Task CreateSpecificMaintenanceAsync_WithValidModel_ReturnsTrue()
        {
            // Arrange
            var model = new SMCreateViewModel
            {
                Name = "New Maintenance",
                Description = "Description",
                EquipmentId = Guid.NewGuid().ToString(),
                Interval = 30,
                ResponsiblePosition = "Technician"
            };

            _mockSMRepo.Setup(r => r.AddAsync(It.IsAny<SpecificMaintenance>())).ReturnsAsync(true);

            // Act
            var result = await _smService.CreateSpecificMaintenanceAsync(model, "user123");

            // Assert
            Assert.IsTrue(result);
            _mockSMRepo.Verify(r => r.AddAsync(It.Is<SpecificMaintenance>(sm =>
                sm.Name == "New Maintenance" &&
                sm.Description == "Description" &&
                sm.EquipmentId == Guid.Parse(model.EquipmentId) &&
                sm.Interval == 30 &&
                sm.ResponsiblePosition == "Technician")), Times.Once);
        }

        [Test]
        public async Task GetDetailsAsync_WithValidId_ReturnsDetails()
        {
            // Arrange
            var smId = Guid.NewGuid();
            var specificMaintenance = new SpecificMaintenance
            {
                SpecMaintId = smId,
                Name = "Maintenance A",
                Description = "Description A",
                Equipment = new Equipment { Name = "Equipment A" },
                LastCompletedDate = DateTime.Now,
                Interval = 30,
                ResponsiblePosition = "Technician",
                Creator = new PMSUser { UserName = "UserA" },
                CreatedOn = DateTime.Now.AddDays(-5),
                EditedOn = DateTime.Now,
                IsDeleted = false
            };

            var mockDbSet = new List<SpecificMaintenance> { specificMaintenance }.AsQueryable().BuildMockDbSet();
            _mockSMRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _smService.GetDetailsAsync(smId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Maintenance A", result.Name);
            Assert.AreEqual("Equipment A", result.Equipment);
        }

        [Test]
        public async Task SaveItemToEditAsync_WithValidModel_ReturnsTrue()
        {
            // Arrange
            var smId = Guid.NewGuid();
            var specificMaintenance = new SpecificMaintenance
            {
                SpecMaintId = smId,
                Name = "Old Maintenance",
                Description = "Old Description",
                Interval = 30,
                ResponsiblePosition = "Technician",
                IsDeleted = false
            };

            var editModel = new SMEditViewModel
            {
                SMId = smId.ToString(),
                Name = "Updated Maintenance",
                Description = "Updated Description",
                Interval = 60,
                ResponsiblePosition = "Engineer"
            };

            var mockDbSet = new List<SpecificMaintenance> { specificMaintenance }.AsQueryable().BuildMockDbSet();
            _mockSMRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);
            _mockSMRepo.Setup(r => r.UpdateAsync(It.IsAny<SpecificMaintenance>())).ReturnsAsync(true);

            // Act
            var result = await _smService.SaveItemToEditAsync(editModel, "user123");

            // Assert
            Assert.IsTrue(result);
            _mockSMRepo.Verify(r => r.UpdateAsync(It.Is<SpecificMaintenance>(sm =>
                sm.Name == "Updated Maintenance" &&
                sm.Description == "Updated Description" &&
                sm.Interval == 60 &&
                sm.ResponsiblePosition == "Engineer")), Times.Once);
        }

        [Test]
        public async Task ConfirmDeleteAsync_WithValidModel_ReturnsTrue()
        {
            // Arrange
            var smId = Guid.NewGuid();
            var specificMaintenance = new SpecificMaintenance
            {
                SpecMaintId = smId,
                Name = "Maintenance A",
                IsDeleted = false
            };

            var mockDbSet = new List<SpecificMaintenance> { specificMaintenance }.AsQueryable().BuildMockDbSet();
            _mockSMRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);
            _mockSMRepo.Setup(r => r.UpdateAsync(It.IsAny<SpecificMaintenance>())).ReturnsAsync(true);

            var deleteModel = new SMDeleteViewModel { SmId = smId.ToString(), Name = "Maintenance A" };

            // Act
            var result = await _smService.ConfirmDeleteAsync(deleteModel);

            // Assert
            Assert.IsTrue(result);
            _mockSMRepo.Verify(r => r.UpdateAsync(It.Is<SpecificMaintenance>(sm => sm.IsDeleted)), Times.Once);
        }
    }
}
