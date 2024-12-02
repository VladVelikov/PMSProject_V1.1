using Microsoft.AspNetCore.Mvc;
using Moq;
using PMS.Services.Data.Interfaces;
using PMSWeb.Controllers;
using PMSWeb.ViewModels.InventoryVM;

namespace PMSTests.Controllers
{
    [TestFixture]
    public class InventoryControllerTests
    {
        private Mock<IInventoryService> _inventoryServiceMock;
        private InventoryController _controller;

        [SetUp]
        public void SetUp()
        {
            _inventoryServiceMock = new Mock<IInventoryService>();
            _controller = new InventoryController(_inventoryServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test]
        public async Task SparesInventory_ReturnsRedirectToEmptyList_WhenNoSparesExist()
        {
            // Arrange
            _inventoryServiceMock.Setup(service => service.GetSparesInventoryViewModelAsync())
                .ReturnsAsync((SparesInventoryViewModel)null);

            // Act
            var result = await _controller.SparesInventory();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("EmptyList", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task SparesInventory_ReturnsViewWithModel_WhenDataExists()
        {
            // Arrange
            var model = new SparesInventoryViewModel
            {
                Spares = new List<InventoryItemViewModel>
            {
                new InventoryItemViewModel { Id = Guid.NewGuid().ToString(), Name = "Spare 1", Used = 5 },
                new InventoryItemViewModel { Id = Guid.NewGuid().ToString(), Name = "Spare 2", Used = 10 }
            }
            };
            _inventoryServiceMock.Setup(service => service.GetSparesInventoryViewModelAsync()).ReturnsAsync(model);

            // Act
            var result = await _controller.SparesInventory();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task UpdateSparesInventory_InvalidModel_RedirectsToSparesInventory()
        {
            // Arrange
            var model = new SparesInventoryViewModel();
            _controller.ModelState.AddModelError("Spares", "Required");

            // Act
            var result = await _controller.UpdateSparesInventory(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(InventoryController.SparesInventory), redirectResult.ActionName);
        }

        [Test]
        public async Task UpdateSparesInventory_InvalidData_RedirectsToWrongData()
        {
            // Arrange
            var model = new SparesInventoryViewModel
            {
                Spares = new List<InventoryItemViewModel>
            {
                new InventoryItemViewModel { Id = "invalid-guid", Used = 5 }
            }
            };

            // Act
            var result = await _controller.UpdateSparesInventory(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("WrongData", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task UpdateSparesInventory_Successful_RedirectsToSparesInventory()
        {
            // Arrange
            var model = new SparesInventoryViewModel
            {
                Spares = new List<InventoryItemViewModel>
            {
                new InventoryItemViewModel { Id = Guid.NewGuid().ToString(), Used = 5 }
            }
            };
            _inventoryServiceMock.Setup(service => service.UpdateSparesInventoryAsync(model)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateSparesInventory(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(InventoryController.SparesInventory), redirectResult.ActionName);
        }

        [Test]
        public async Task ConsumablesInventory_ReturnsRedirectToEmptyList_WhenNoConsumablesExist()
        {
            // Arrange
            _inventoryServiceMock.Setup(service => service.GetConsumablesInventoryViewModelAsync())
                .ReturnsAsync((ConsumablesInventoryViewModel)null);

            // Act
            var result = await _controller.ConsumablesInventory();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("EmptyList", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task ConsumablesInventory_ReturnsViewWithModel_WhenDataExists()
        {
            // Arrange
            var model = new ConsumablesInventoryViewModel
            {
                Consumables = new List<InventoryItemViewModel>
            {
                new InventoryItemViewModel { Id = Guid.NewGuid().ToString(), Name = "Consumable 1", Used = 5 },
                new InventoryItemViewModel { Id = Guid.NewGuid().ToString(), Name = "Consumable 2", Used = 10 }
            }
            };
            _inventoryServiceMock.Setup(service => service.GetConsumablesInventoryViewModelAsync()).ReturnsAsync(model);

            // Act
            var result = await _controller.ConsumablesInventory();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task UpdateConsumablesInventory_InvalidModel_RedirectsToConsumablesInventory()
        {
            // Arrange
            var model = new ConsumablesInventoryViewModel();
            _controller.ModelState.AddModelError("Consumables", "Required");

            // Act
            var result = await _controller.UpdateConsumablesInventory(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(InventoryController.ConsumablesInventory), redirectResult.ActionName);
        }

        [Test]
        public async Task UpdateConsumablesInventory_Successful_RedirectsToConsumablesInventory()
        {
            // Arrange
            var model = new ConsumablesInventoryViewModel
            {
                Consumables = new List<InventoryItemViewModel>
            {
                new InventoryItemViewModel { Id = Guid.NewGuid().ToString(), Used = 5 }
            }
            };
            _inventoryServiceMock.Setup(service => service.UpdateConsumablesInventoryAsync(model)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateConsumablesInventory(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(InventoryController.ConsumablesInventory), redirectResult.ActionName);
        }
    }

}
