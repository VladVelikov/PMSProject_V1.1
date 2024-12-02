using Microsoft.EntityFrameworkCore;
using MockQueryable;
using Moq;
using NUnit.Framework;
using PMS.Data.Models;
using PMS.Data.Models.Identity;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data;
using PMSWeb.ViewModels.Consumable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMSTests.Services
{
    [TestFixture]
    public class ConsumableServiceTests
    {
        private Mock<IRepository<Consumable, Guid>> _mockRepository;
        private ConsumableService _consumableService;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IRepository<Consumable, Guid>>();
            _consumableService = new ConsumableService(_mockRepository.Object);
        }

        #region CreateConsumableAsync Tests

        [Test]
        public async Task CreateConsumableAsync_WithValidModel_ReturnsTrue()
        {
            // Arrange
            var model = new ConsumableCreateViewModel
            {
                Name = "Test Consumable",
                Units = "kg",
                Description = "Test Description",
                Price = 10.5m,
                ROB = 100
            };
            string userId = Guid.NewGuid().ToString();

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Consumable>())).ReturnsAsync(true);

            // Act
            var result = await _consumableService.CreateConsumableAsync(model, userId);

            // Assert
            Assert.IsTrue(result);
            _mockRepository.Verify(r => r.AddAsync(It.Is<Consumable>(c => c.Name == "Test Consumable")), Times.Once);
        }

        [Test]
        public async Task CreateConsumableAsync_WithRepositoryException_ReturnsFalse()
        {
            // Arrange
            var model = new ConsumableCreateViewModel
            {
                Name = "Test Consumable",
                Units = "kg",
                Description = "Test Description",
                Price = 10.5m,
                ROB = 100
            };
            string userId = Guid.NewGuid().ToString();

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Consumable>())).ThrowsAsync(new Exception());

            // Act
            var result = await _consumableService.CreateConsumableAsync(model, userId);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region GetDetailsAsync Tests

        [Test]
        public async Task GetDetailsAsync_WithValidId_ReturnsDetails()
        {
            // Arrange
            var consumableId = Guid.NewGuid();
            var consumables = new List<Consumable>
            {
                new Consumable
                {
                    ConsumableId = consumableId,
                    Name = "Test Consumable",
                    Units = "kg",
                    Description = "Description",
                    Price = 10.5m,
                    ROB = 100,
                    CreatorId = Guid.NewGuid().ToString(),
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
                    Creator = new PMSUser { UserName = "Test User" },
                    IsDeleted = false
                }
            };
            _mockRepository.Setup(r => r.GetAllAsQueryable()).Returns(consumables.AsQueryable().BuildMock());

            // Act
            var result = await _consumableService.GetDetailsAsync(consumableId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(consumableId.ToString(), result.ConsumableId);
            Assert.AreEqual("Test Consumable", result.Name);
        }

        [Test]
        public async Task GetDetailsAsync_WithInvalidId_ReturnsEmptyModel()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsQueryable()).Returns(new List<Consumable>().AsQueryable().BuildMock());

            // Act
            var result = await _consumableService.GetDetailsAsync(Guid.NewGuid().ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Name);
        }

        #endregion

        #region GetListOfViewModelsAsync Tests

        [Test]
        public async Task GetListOfViewModelsAsync_ReturnsSortedConsumables()
        {
            // Arrange
            var consumables = new List<Consumable>
            {
                new Consumable { Name = "Consumable A", EditedOn = DateTime.Now.AddDays(-1), IsDeleted = false },
                new Consumable { Name = "Consumable B", EditedOn = DateTime.Now, IsDeleted = false }
            };
            _mockRepository.Setup(r => r.GetAllAsQueryable()).Returns(consumables.AsQueryable().BuildMock());

            // Act
            var result = await _consumableService.GetListOfViewModelsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Consumable B", result.First().Name);
        }

        #endregion

        #region SaveItemToEditAsync Tests

        [Test]
        public async Task SaveItemToEditAsync_WithValidModel_ReturnsTrue()
        {
            // Arrange
            var consumableId = Guid.NewGuid();
            var consumable = new Consumable { ConsumableId = consumableId, Name = "Original Name", IsDeleted = false };
            var model = new ConsumableEditViewModel
            {
                ConsumableId = consumableId.ToString(),
                Name = "Updated Name",
                Units = "kg",
                Description = "Updated Description",
                Price = 20.0m,
                ROB = 200
            };

            _mockRepository.Setup(r => r.GetAllAsQueryable()).Returns(new List<Consumable> { consumable }.AsQueryable().BuildMock());
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Consumable>())).ReturnsAsync(true);
            // Act
            var result = await _consumableService.SaveItemToEditAsync(model, Guid.NewGuid().ToString());

            // Assert
            Assert.IsTrue(result);
            _mockRepository.Verify(r => r.UpdateAsync(It.Is<Consumable>(c => c.Name == "Updated Name")), Times.Once);
        }

        [Test]
        public async Task SaveItemToEditAsync_WithInvalidId_ReturnsFalse()
        {
            // Arrange
            var model = new ConsumableEditViewModel
            {
                ConsumableId = Guid.NewGuid().ToString(),
                Name = "Updated Name"
            };

            _mockRepository.Setup(r => r.GetAllAsQueryable()).Returns(new List<Consumable>().AsQueryable().BuildMock());

            // Act
            var result = await _consumableService.SaveItemToEditAsync(model, Guid.NewGuid().ToString());

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region ConfirmDeleteAsync Tests

        [Test]
        public async Task ConfirmDeleteAsync_WithValidModel_ReturnsTrue()
        {
            // Arrange
            var consumableId = Guid.NewGuid();
            var consumable = new Consumable { ConsumableId = consumableId, IsDeleted = false };

            _mockRepository.Setup(r => r.GetAllAsQueryable()).Returns(new List<Consumable> { consumable }.AsQueryable().BuildMock());
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Consumable>())).ReturnsAsync(true);

            var model = new ConsumableDeleteViewModel { ConsumableId = consumableId.ToString() };

            // Act
            var result = await _consumableService.ConfirmDeleteAsync(model);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ConfirmDeleteAsync_WithInvalidId_ReturnsFalse()
        {
            // Arrange
            var model = new ConsumableDeleteViewModel { ConsumableId = Guid.NewGuid().ToString() };

            _mockRepository.Setup(r => r.GetAllAsQueryable()).Returns(new List<Consumable>().AsQueryable().BuildMock());

            // Act
            var result = await _consumableService.ConfirmDeleteAsync(model);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion
    }
}
