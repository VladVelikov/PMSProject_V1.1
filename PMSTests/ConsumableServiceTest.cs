namespace PMSTests
{

    using Moq;
    using NUnit.Framework;
    using PMS.Data.Models;
    using PMS.Data.Repository.Interfaces;
    using PMS.Services.Data;
    using PMSWeb.ViewModels.Consumable;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    [TestFixture]
    public class ConsumableServiceTests
    {
        private Mock<IRepository<Consumable, Guid>> _mockRepository;
        private ConsumableService _service;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IRepository<Consumable, Guid>>();
            _service = new ConsumableService(_mockRepository.Object);
        }

        [Test]
        public async Task CreateConsumableAsync_ShouldReturnTrue_WhenConsumableIsCreatedSuccessfully()
        {
            // Arrange
            var model = new ConsumableCreateViewModel
            {
                Name = "Test Consumable",
                Units = "pcs",
                Description = "Test Description",
                Price = 10.5m,
                ROB = 50
            };
            string userId = "user123";

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Consumable>()))
                .Returns(Task.FromResult(true));

            // Act
            var result = await _service.CreateConsumableAsync(model, userId);

            // Assert
            Assert.IsTrue(result);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Consumable>()), Times.Once);
        }

        [Test]
        public async Task CreateConsumableAsync_ShouldReturnFalse_WhenExceptionIsThrown()
        {
            // Arrange
            var model = new ConsumableCreateViewModel
            {
                Name = "Test Consumable",
                Units = "pcs",
                Description = "Test Description",
                Price = 10.5m,
                ROB = 50
            };
            string userId = "user123";

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Consumable>()))
                .Throws(new Exception());

            // Act
            var result = await _service.CreateConsumableAsync(model, userId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetDetailsAsync_ShouldReturnDetails_WhenConsumableExists()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var consumable = new Consumable
            {
                ConsumableId = Guid.Parse(id),
                Name = "Test Consumable",
                Units = "pcs",
                Description = "Test Description",
                Price = 10.5m,
                ROB = 50,
                CreatorId = "user123",
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now,
                IsDeleted = false
            };
            _mockRepository.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<Consumable> { consumable }.AsQueryable());

            // Act
            var result = await _service.GetDetailsAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Test Consumable", result.Name);
            _mockRepository.Verify(r => r.GetAllAsQueryable(), Times.Once);
        }

        [Test]
        public async Task GetDetailsAsync_ShouldReturnEmptyViewModel_WhenConsumableDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            _mockRepository.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<Consumable>().AsQueryable());

            // Act
            var result = await _service.GetDetailsAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(string.Empty, result.Name);
        }

        [Test]
        public async Task SaveItemToEditAsync_ShouldReturnTrue_WhenConsumableIsUpdatedSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var consumable = new Consumable
            {
                ConsumableId = Guid.Parse(id),
                Name = "Old Name",
                Units = "pcs",
                Description = "Old Description",
                Price = 10.5m,
                ROB = 50,
                CreatorId = "user123",
                EditedOn = DateTime.Now,
                IsDeleted = false
            };
            var model = new ConsumableEditViewModel
            {
                ConsumableId = id,
                Name = "New Name",
                Units = "pcs",
                Description = "New Description",
                Price = 15.5m,
                ROB = 60
            };

            _mockRepository.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<Consumable> { consumable }.AsQueryable());
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Consumable>()))
                .Returns(Task.FromResult(true));

            // Act
            var result = await _service.SaveItemToEditAsync(model, "user123");

            // Assert
            Assert.IsTrue(result);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Consumable>()), Times.Once);
        }

        [Test]
        public async Task ConfirmDeleteAsync_ShouldReturnTrue_WhenConsumableIsDeletedSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var consumable = new Consumable
            {
                ConsumableId = Guid.Parse(id),
                IsDeleted = false
            };
            var model = new ConsumableDeleteViewModel { ConsumableId = id };

            _mockRepository.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<Consumable> { consumable }.AsQueryable());
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Consumable>()))
                .Returns(Task.FromResult(true));

            // Act
            var result = await _service.ConfirmDeleteAsync(model);

            // Assert
            Assert.IsTrue(result);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Consumable>()), Times.Once);
        }

        [Test]
        public async Task ConfirmDeleteAsync_ShouldReturnFalse_WhenConsumableDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var model = new ConsumableDeleteViewModel { ConsumableId = id };

            _mockRepository.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<Consumable>().AsQueryable());

            // Act
            var result = await _service.ConfirmDeleteAsync(model);

            // Assert
            Assert.IsFalse(result);
        }
    }
}

