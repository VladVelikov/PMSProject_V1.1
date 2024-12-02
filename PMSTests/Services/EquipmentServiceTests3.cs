using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using MockQueryable.Moq;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data;
using PMSWeb.ViewModels.Equipment;
using PMSWeb.ViewModels.CommonVM;
using MockQueryable;
using PMS.Data.Models.Identity;

namespace PMSTests.Services
{
    [TestFixture]
    public class EquipmentServiceTests3
    {
        private Mock<IRepository<Equipment, Guid>> _equipmentRepoMock;
        private Mock<IRepository<Maker, Guid>> _makersRepoMock;
        private Mock<IRepository<RoutineMaintenance, Guid>> _routineMaintenancesRepoMock;
        private Mock<IRepository<Consumable, Guid>> _consumablesRepoMock;
        private Mock<IRepository<RoutineMaintenanceEquipment, Guid[]>> _routineMaintenanceEquipmentRepoMock;
        private Mock<IRepository<ConsumableEquipment, Guid[]>> _consumableEquipmentRepoMock;
        private Mock<IRepository<SpecificMaintenance, Guid>> _specificMaintenancesRepoMock;
        private Mock<IRepository<Sparepart, Guid>> _sparePartsRepoMock;
        private Mock<IRepository<Manual, Guid>> _manualsRepoMock;

        private EquipmentService _service;

        [SetUp]
        public void SetUp()
        {
            _equipmentRepoMock = new Mock<IRepository<Equipment, Guid>>();
            _makersRepoMock = new Mock<IRepository<Maker, Guid>>();
            _routineMaintenancesRepoMock = new Mock<IRepository<RoutineMaintenance, Guid>>();
            _consumablesRepoMock = new Mock<IRepository<Consumable, Guid>>();
            _routineMaintenanceEquipmentRepoMock = new Mock<IRepository<RoutineMaintenanceEquipment, Guid[]>>();
            _consumableEquipmentRepoMock = new Mock<IRepository<ConsumableEquipment, Guid[]>>();
            _specificMaintenancesRepoMock = new Mock<IRepository<SpecificMaintenance, Guid>>();
            _sparePartsRepoMock = new Mock<IRepository<Sparepart, Guid>>();
            _manualsRepoMock = new Mock<IRepository<Manual, Guid>>();

            _service = new EquipmentService(
                _equipmentRepoMock.Object,
                _makersRepoMock.Object,
                _routineMaintenancesRepoMock.Object,
                _consumablesRepoMock.Object,
                _routineMaintenanceEquipmentRepoMock.Object,
                _consumableEquipmentRepoMock.Object,
                _specificMaintenancesRepoMock.Object,
                _sparePartsRepoMock.Object,
                _manualsRepoMock.Object
            );
        }

        [Test]
        public async Task ConfirmDeleteAsync_ReturnsTrue_WhenDeletionSucceeds()
        {
            // Arrange
            var equipmentId = Guid.NewGuid();
            var equipment = new Equipment { EquipmentId = equipmentId, IsDeleted = false };
            var equipmentList = new List<Equipment> { equipment }.AsQueryable().BuildMock();

            _equipmentRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(equipmentList);
            _equipmentRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Equipment>())).ReturnsAsync(true);

            var model = new EquipmentDeleteViewModel { EquipmentId = equipmentId.ToString() };

            // Act
            var result = await _service.ConfirmDeleteAsync(model);

            // Assert
            Assert.IsTrue(result);
            _equipmentRepoMock.Verify(repo => repo.UpdateAsync(It.Is<Equipment>(e => e.IsDeleted)), Times.Once);
        }

        [Test]
        public async Task GetCreateModelAsync_ReturnsPopulatedModel_WhenDataExists()
        {
            // Arrange
            var makers = new List<Maker>
            {
                new Maker { MakerId = Guid.NewGuid(), MakerName = "Maker1", IsDeleted = false },
                new Maker { MakerId = Guid.NewGuid(), MakerName = "Maker2", IsDeleted = false }
            }.AsQueryable().BuildMock();

            var routineMaintenances = new List<RoutineMaintenance>
            {
                new RoutineMaintenance { RoutMaintId = Guid.NewGuid(), Name = "Maintenance1", IsDeleted = false },
                new RoutineMaintenance { RoutMaintId = Guid.NewGuid(), Name = "Maintenance2", IsDeleted = false }
            }.AsQueryable().BuildMock();

            var consumables = new List<Consumable>
            {
                new Consumable { ConsumableId = Guid.NewGuid(), Name = "Consumable1", IsDeleted = false },
                new Consumable { ConsumableId = Guid.NewGuid(), Name = "Consumable2", IsDeleted = false }
            }.AsQueryable().BuildMock();

            _makersRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(makers);
            _routineMaintenancesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(routineMaintenances);
            _consumablesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(consumables);

            // Act
            var result = await _service.GetCreateModelAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Makers.Count);
            Assert.AreEqual(2, result.RoutineMaintenances.Count);
            Assert.AreEqual(2, result.Consumables.Count);
        }

        [Test]
        public async Task CreateEquipmentAsync_ReturnsTrue_WhenCreationSucceeds()
        {
            // Arrange
            var model = new EquipmentCreateViewModel
            {
                Name = "Test Equipment",
                Description = "Test Description",
                MakerId = Guid.NewGuid()
            };

            var userId = Guid.NewGuid().ToString();
            var routineMaintenances = new List<Guid> { Guid.NewGuid() };
            var consumables = new List<Guid> { Guid.NewGuid() };

            var routineMaintenanceEquipmentMock = new List<RoutineMaintenanceEquipment>().AsQueryable().BuildMock();
            var consumableEquipmentMock = new List<ConsumableEquipment>().AsQueryable().BuildMock();

            _routineMaintenanceEquipmentRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(routineMaintenanceEquipmentMock);
            _consumableEquipmentRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(consumableEquipmentMock);

            _equipmentRepoMock.Setup(repo => repo.AddAsync(It.IsAny<Equipment>())).ReturnsAsync(true);
            _routineMaintenanceEquipmentRepoMock.Setup(repo => repo.AddAsync(It.IsAny<RoutineMaintenanceEquipment>())).ReturnsAsync(true);
            _consumableEquipmentRepoMock.Setup(repo => repo.AddAsync(It.IsAny<ConsumableEquipment>())).ReturnsAsync(true);

            // Act
            var result = await _service.CreateEquipmentAsync(model, userId, routineMaintenances, consumables);

            // Assert
            Assert.IsTrue(result);
            _equipmentRepoMock.Verify(repo => repo.AddAsync(It.IsAny<Equipment>()), Times.Once);
            _routineMaintenanceEquipmentRepoMock.Verify(repo => repo.AddAsync(It.IsAny<RoutineMaintenanceEquipment>()), Times.Exactly(routineMaintenances.Count));
            _consumableEquipmentRepoMock.Verify(repo => repo.AddAsync(It.IsAny<ConsumableEquipment>()), Times.Exactly(consumables.Count));
        }


        [Test]
        public async Task GetDetailsAsync_ReturnsPopulatedModel_WhenIdIsValid()
        {
            // Arrange
            var validId = Guid.NewGuid();
            var equipment = new Equipment
            {
                EquipmentId = validId,
                Name = "Test Equipment",
                Description = "Test Description",
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now,
                Maker = new Maker { MakerName = "Test Maker" },
                Creator = new PMSUser { UserName = "Test Creator" },
                IsDeleted = false
            };

            var routineMaintenanceEquipments = new List<RoutineMaintenanceEquipment>
    {
        new() { EquipmentId = validId, RoutineMaintenance = new RoutineMaintenance { Name = "Maintenance 1" } },
        new() { EquipmentId = validId, RoutineMaintenance = new RoutineMaintenance { Name = "Maintenance 2" } }
    };

            var specificMaintenances = new List<SpecificMaintenance>
    {
        new() { EquipmentId = validId, Name = "Specific Maintenance 1" },
        new() { EquipmentId = validId, Name = "Specific Maintenance 2" }
    };

            var spareParts = new List<Sparepart>
    {
        new() { EquipmentId = validId, SparepartName = "Spare Part 1" },
        new() { EquipmentId = validId, SparepartName = "Spare Part 2" }
    };

            var manuals = new List<Manual>
    {
        new() { EquipmentId = validId, ManualName = "Manual 1" },
        new() { EquipmentId = validId, ManualName = "Manual 2" }
    };

            var consumableEquipments = new List<ConsumableEquipment>
    {
        new() { EquipmentId = validId, Consumable = new Consumable { Name = "Consumable 1", IsDeleted = false } },
        new() { EquipmentId = validId, Consumable = new Consumable { Name = "Consumable 2", IsDeleted = false } }
    };

            // Mock IQueryable data
            _equipmentRepoMock.Setup(repo => repo.GetAllAsQueryable())
                .Returns(new List<Equipment> { equipment }.AsQueryable().BuildMock());

            _routineMaintenanceEquipmentRepoMock.Setup(repo => repo.GetAllAsQueryable())
                .Returns(routineMaintenanceEquipments.AsQueryable().BuildMock());

            _specificMaintenancesRepoMock.Setup(repo => repo.GetAllAsQueryable())
                .Returns(specificMaintenances.AsQueryable().BuildMock());

            _sparePartsRepoMock.Setup(repo => repo.GetAllAsQueryable())
                .Returns(spareParts.AsQueryable().BuildMock());

            _manualsRepoMock.Setup(repo => repo.GetAllAsQueryable())
                .Returns(manuals.AsQueryable().BuildMock());

            _consumableEquipmentRepoMock.Setup(repo => repo.GetAllAsQueryable())
                .Returns(consumableEquipments.AsQueryable().BuildMock());

            // Act
            var result = await _service.GetDetailsAsync(validId.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(validId.ToString(), result.EquipmentId);
            Assert.AreEqual("Test Equipment", result.Name);
            Assert.AreEqual("Test Description", result.Description);
            Assert.AreEqual("Test Maker", result.Maker);
            Assert.AreEqual("Test Creator", result.Creator);
            Assert.AreEqual(2, result.RoutineMaintenances.Count);
            Assert.AreEqual(2, result.SpecificMaintenances.Count);
            Assert.AreEqual(2, result.SpareParts.Count);
            Assert.AreEqual(2, result.Manuals.Count);
            Assert.AreEqual(2, result.Consumables.Count);
        }

        [Test]
        public async Task ConfirmDeleteAsync_ReturnsFalse_WhenModelIsNull()
        {
            // Act
            var result = await _service.ConfirmDeleteAsync(null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ConfirmDeleteAsync_ReturnsFalse_WhenEquipmentIdIsNull()
        {
            // Act
            var result = await _service.ConfirmDeleteAsync(new EquipmentDeleteViewModel { EquipmentId = null });

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ConfirmDeleteAsync_ReturnsFalse_WhenNoMatchingRecordFound()
        {
            // Arrange
            _equipmentRepoMock.Setup(repo => repo.GetAllAsQueryable())
                .Returns(new List<Equipment>().AsQueryable().BuildMock());

            var model = new EquipmentDeleteViewModel { EquipmentId = Guid.NewGuid().ToString() };

            // Act
            var result = await _service.ConfirmDeleteAsync(model);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ConfirmDeleteAsync_ReturnsFalse_WhenUpdateFails()
        {
            // Arrange
            var equipment = new Equipment
            {
                EquipmentId = Guid.NewGuid(),
                IsDeleted = false
            };

            _equipmentRepoMock.Setup(repo => repo.GetAllAsQueryable())
                .Returns(new List<Equipment> { equipment }.AsQueryable().BuildMock());

            _equipmentRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Equipment>()))
                .ThrowsAsync(new Exception("Update failed"));

            var model = new EquipmentDeleteViewModel { EquipmentId = equipment.EquipmentId.ToString() };

            // Act
            var result = await _service.ConfirmDeleteAsync(model);

            // Assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task GetCreateModelAsync_ReturnsEmptyLists_WhenNoDataAvailable()
        {
            // Arrange
            _makersRepoMock.Setup(repo => repo.GetAllAsQueryable())
                .Returns(new List<Maker>().AsQueryable().BuildMock());
            _routineMaintenancesRepoMock.Setup(repo => repo.GetAllAsQueryable())
                .Returns(new List<RoutineMaintenance>().AsQueryable().BuildMock());
            _consumablesRepoMock.Setup(repo => repo.GetAllAsQueryable())
                .Returns(new List<Consumable>().AsQueryable().BuildMock());

            // Act
            var result = await _service.GetCreateModelAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.Makers);
            Assert.IsEmpty(result.RoutineMaintenances);
            Assert.IsEmpty(result.Consumables);
        }
        [Test]
        public async Task CreateEquipmentAsync_ReturnsTrue_WhenNoRoutineMaintenancesOrConsumables()
        {
            // Arrange
            var model = new EquipmentCreateViewModel
            {
                Name = "New Equipment",
                Description = "Test Description",
                MakerId = Guid.NewGuid()
            };

            var userId = Guid.NewGuid().ToString();

            _equipmentRepoMock.Setup(repo => repo.AddAsync(It.IsAny<Equipment>()))
                .ReturnsAsync(true);

            _routineMaintenanceEquipmentRepoMock.Setup(repo => repo.GetAllAsQueryable())
                .Returns(new List<RoutineMaintenanceEquipment>().AsQueryable().BuildMock());

            _consumableEquipmentRepoMock.Setup(repo => repo.GetAllAsQueryable())
                .Returns(new List<ConsumableEquipment>().AsQueryable().BuildMock());

            // Act
            var result = await _service.CreateEquipmentAsync(model, userId, new List<Guid>(), new List<Guid>());

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task SaveItemToEditAsync_ReturnsFalse_WhenNoMatchingEquipmentFound()
        {
            // Arrange
            _equipmentRepoMock.Setup(repo => repo.GetAllAsQueryable())
                .Returns(new List<Equipment>().AsQueryable().BuildMock());

            var model = new EquipmentEditViewModel { EquipmentId = Guid.NewGuid().ToString() };

            // Act
            var result = await _service.SaveItemToEditAsync(model, Guid.NewGuid().ToString(), new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>());

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetListOfViewModelsAsync_ReturnsEmptyList_WhenNoDataAvailable()
        {
            // Arrange
            _equipmentRepoMock.Setup(repo => repo.GetAllAsQueryable())
                .Returns(new List<Equipment>().AsQueryable().BuildMock());

            // Act
            var result = await _service.GetListOfViewModelsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task ConfirmDeleteAsync_ReturnsFalse_WhenRecordAlreadyDeleted()
        {
            // Arrange
            var equipment = new Equipment
            {
                EquipmentId = Guid.NewGuid(),
                IsDeleted = true
            };

            _equipmentRepoMock.Setup(repo => repo.GetAllAsQueryable())
                .Returns(new List<Equipment> { equipment }.AsQueryable().BuildMock());

            var model = new EquipmentDeleteViewModel { EquipmentId = equipment.EquipmentId.ToString() };

            // Act
            var result = await _service.ConfirmDeleteAsync(model);

            // Assert
            Assert.IsFalse(result);
        }

      





    }
}
