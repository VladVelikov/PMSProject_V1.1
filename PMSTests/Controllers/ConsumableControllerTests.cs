using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMS.Services.Data.Interfaces;
using PMSWeb.Controllers;
using PMSWeb.ViewModels.Consumable;
using System.Security.Claims;

namespace PMSTests.Controllers
{
    [TestFixture]
    public class ConsumableControllerTests : IDisposable
    {
        private Mock<IConsumableService> _consumableServiceMock;
        private ConsumableController _controller;

        [SetUp]
        public void SetUp()
        {
            _consumableServiceMock = new Mock<IConsumableService>();
            _controller = new ConsumableController(_consumableServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test]
        public async Task Select_ReturnsRedirectToEmptyList_WhenNoDataExists()
        {
            // Arrange
            _consumableServiceMock.Setup(service => service.GetListOfViewModelsAsync())
                .ReturnsAsync(Enumerable.Empty<ConsumableDisplayViewModel>());

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("EmptyList", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Select_ReturnsViewWithModels_WhenDataExists()
        {
            // Arrange
            var models = new[] {
            new ConsumableDisplayViewModel { ConsumableId = Guid.NewGuid().ToString(), Name = "Consumable 1" },
            new ConsumableDisplayViewModel { ConsumableId = Guid.NewGuid().ToString(), Name = "Consumable 2" }
        };
            _consumableServiceMock.Setup(service => service.GetListOfViewModelsAsync()).ReturnsAsync(models);

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(models, viewResult.Model);
        }

        [Test]
        public void Create_Get_ReturnsViewWithEmptyModel()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<ConsumableCreateViewModel>(viewResult.Model);
        }

        [Test]
        public async Task Create_Post_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            var model = new ConsumableCreateViewModel();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task Create_Post_WhenServiceFails_RedirectsToNotCreated()
        {
            // Arrange
            var model = new ConsumableCreateViewModel { Name = "Test Consumable", Price = 10.0M };

            // Mock the service to simulate a failure
            _consumableServiceMock
                .Setup(service => service.CreateConsumableAsync(model, It.IsAny<string>()))
                .ReturnsAsync(false);

            // Mock the user
            var userId = Guid.NewGuid().ToString();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId)
    }));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("NotCreated", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }


        [Test]
        public async Task Create_Post_WhenSuccessful_RedirectsToSelect()
        {
            // Arrange
            var model = new ConsumableCreateViewModel { Name = "Test Consumable", Price = 10.0M };

            // Mock the service to simulate successful creation
            _consumableServiceMock
                .Setup(service => service.CreateConsumableAsync(model, It.IsAny<string>()))
                .ReturnsAsync(true);

            // Mock the user
            var userId = Guid.NewGuid().ToString();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId)
    }));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(ConsumableController.Select), redirectResult.ActionName);
        }


        public void Dispose()
        {
            _controller?.Dispose();
        }
    }
}
