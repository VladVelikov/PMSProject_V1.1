using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using PMS.Data.Models;
using PMS.Data.Models.Identity;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data;
using PMSWeb.ViewModels.RM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMSTests.Services
{
    [TestFixture]
    public class RMServiceTests
    {
        private Mock<IRepository<RoutineMaintenance, Guid>> _mockRMsRepo;
        private Mock<IRepository<RoutineMaintenanceEquipment, Guid[]>> _mockRoutineMaintenanceEquipmentsRepo;
        private RMService _rmService;

        [SetUp]
        public void Setup()
        {
            _mockRMsRepo = new Mock<IRepository<RoutineMaintenance, Guid>>();
            _mockRoutineMaintenanceEquipmentsRepo = new Mock<IRepository<RoutineMaintenanceEquipment, Guid[]>>();

            _rmService = new RMService(
                _mockRMsRepo.Object,
                _mockRoutineMaintenanceEquipmentsRepo.Object
            );
        }

        [Test]
        public async Task GetListOfViewModelsAsync_ReturnsListOfRMs()
        {
            // Arrange
            var routineMaintenances = new List<RoutineMaintenance>
            {
                new RoutineMaintenance
                {
                    RoutMaintId = Guid.NewGuid(),
                    Name = "Routine Maintenance A",
                    Description = "Description A",
                    LastCompletedDate = DateTime.Now.AddDays(-1),
                    Interval = 30,
                    ResponsiblePosition = "Engineer",
                    EditedOn = DateTime.Now,
                    IsDeleted = false
                },
                new RoutineMaintenance
                {
                    RoutMaintId = Guid.NewGuid(),
                    Name = "Routine Maintenance B",
                    Description = "Description B",
                    LastCompletedDate = DateTime.Now.AddDays(-2),
                    Interval = 60,
                    ResponsiblePosition = "Technician",
                    EditedOn = DateTime.Now.AddDays(-1),
                    IsDeleted = false
                }
            };

            var mockDbSet = routineMaintenances.AsQueryable().BuildMockDbSet();
            _mockRMsRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _rmService.GetListOfViewModelsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Routine Maintenance A", result.First().Name);
        }

        [Test]
        public async Task CreateRMAsync_WithValidModel_ReturnsTrue()
        {
            // Arrange
            var model = new RMCreateViewModel
            {
                Name = "New Routine Maintenance",
                Description = "New RM Description",
                Interval = 30,
                ResponsiblePosition = "Technician"
            };

            _mockRMsRepo.Setup(r => r.AddAsync(It.IsAny<RoutineMaintenance>())).ReturnsAsync(true);

            // Act
            var result = await _rmService.CreateRMAsync(model, "user123");

            // Assert
            Assert.IsTrue(result);
            _mockRMsRepo.Verify(r => r.AddAsync(It.Is<RoutineMaintenance>(rm =>
                rm.Name == "New Routine Maintenance" &&
                rm.Description == "New RM Description" &&
                rm.Interval == 30 &&
                rm.ResponsiblePosition == "Technician")), Times.Once);
        }

        [Test]
        public async Task GetDetailsAsync_WithValidId_ReturnsDetails()
        {
            // Arrange
            var rmId = Guid.NewGuid();
            var routineMaintenance = new RoutineMaintenance
            {
                RoutMaintId = rmId,
                Name = "Routine Maintenance A",
                Description = "Description A",
                LastCompletedDate = DateTime.Now.AddDays(-1),
                Interval = 30,
                ResponsiblePosition = "Engineer",
                Creator = new PMSUser { UserName = "UserA" },
                CreatedOn = DateTime.Now.AddDays(-5),
                EditedOn = DateTime.Now,
                IsDeleted = false
            };

            var equipments = new List<RoutineMaintenanceEquipment>
            {
                new RoutineMaintenanceEquipment
                {
                    RoutineMaintenanceId = rmId,
                    Equipment = new Equipment { Name = "Equipment A" }
                },
                new RoutineMaintenanceEquipment
                {
                    RoutineMaintenanceId = rmId,
                    Equipment = new Equipment { Name = "Equipment B" }
                }
            };

            var mockDbSet = new List<RoutineMaintenance> { routineMaintenance }.AsQueryable().BuildMockDbSet();
            _mockRMsRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            var mockEquipmentsDbSet = equipments.AsQueryable().BuildMockDbSet();
            _mockRoutineMaintenanceEquipmentsRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockEquipmentsDbSet.Object);

            // Act
            var result = await _rmService.GetDetailsAsync(rmId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Routine Maintenance A", result.Name);
            Assert.AreEqual(2, result.Equipments.Count);
            Assert.Contains("Equipment A", result.Equipments.ToList());
        }

        [Test]
        public async Task ConfirmDeleteAsync_WithValidModel_ReturnsTrue()
        {
            // Arrange
            var rmId = Guid.NewGuid();
            var routineMaintenance = new RoutineMaintenance
            {
                RoutMaintId = rmId,
                Name = "Routine Maintenance A",
                IsDeleted = false
            };

            var mockDbSet = new List<RoutineMaintenance> { routineMaintenance }.AsQueryable().BuildMockDbSet();
            _mockRMsRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);
            _mockRMsRepo.Setup(r => r.UpdateAsync(It.IsAny<RoutineMaintenance>())).ReturnsAsync(true);

            var deleteModel = new RMDeleteViewModel { RmId = rmId.ToString(), Name = "Routine Maintenance A" };

            // Act
            var result = await _rmService.ConfirmDeleteAsync(deleteModel);

            // Assert
            Assert.IsTrue(result);
            _mockRMsRepo.Verify(r => r.UpdateAsync(It.Is<RoutineMaintenance>(rm => rm.IsDeleted)), Times.Once);
        }

        [Test]
        public async Task SaveItemToEditAsync_WithValidModel_ReturnsTrue()
        {
            // Arrange
            var rmId = Guid.NewGuid();
            var routineMaintenance = new RoutineMaintenance
            {
                RoutMaintId = rmId,
                Name = "Old Routine Maintenance",
                Description = "Old Description",
                Interval = 30,
                ResponsiblePosition = "Technician",
                IsDeleted = false
            };

            var editModel = new RMEditViewModel
            {
                RMId = rmId.ToString(),
                Name = "Updated Routine Maintenance",
                Description = "Updated Description",
                Interval = 60,
                ResponsiblePosition = "Engineer"
            };

            var mockDbSet = new List<RoutineMaintenance> { routineMaintenance }.AsQueryable().BuildMockDbSet();
            _mockRMsRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);
            _mockRMsRepo.Setup(r => r.UpdateAsync(It.IsAny<RoutineMaintenance>())).ReturnsAsync(true);

            // Act
            var result = await _rmService.SaveItemToEditAsync(editModel, "user123");

            // Assert
            Assert.IsTrue(result);
            _mockRMsRepo.Verify(r => r.UpdateAsync(It.Is<RoutineMaintenance>(rm =>
                rm.Name == "Updated Routine Maintenance" &&
                rm.Description == "Updated Description" &&
                rm.Interval == 60 &&
                rm.ResponsiblePosition == "Engineer")), Times.Once);
        }
    }
}
