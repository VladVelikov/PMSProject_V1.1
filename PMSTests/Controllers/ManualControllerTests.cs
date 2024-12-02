using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMS.Services.Data.Interfaces;
using PMSWeb.Controllers;
using PMSWeb.ViewModels.Manual;
using System.Security.Claims;

namespace PMSTests.Controllers
{


    [TestFixture]
    public class ManualControllerTests
    {
        private Mock<IManualService> _manualServiceMock;
        private ManualController _controller;

        [SetUp]
        public void SetUp()
        {
            _manualServiceMock = new Mock<IManualService>();
            _controller = new ManualController(_manualServiceMock.Object);

            // Mock user identity
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        }));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test]
        public async Task Select_ReturnsRedirectToNotFound_WhenManualsAreNull()
        {
            // Arrange
            _manualServiceMock.Setup(service => service.GetListOfViewModelsAsync()).ReturnsAsync((List<ManualDisplayViewModel>)null);

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("NotFound", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Select_ReturnsViewWithManuals_WhenDataExists()
        {
            // Arrange
            var manuals = new List<ManualDisplayViewModel>
        {
            new ManualDisplayViewModel { ManualId = Guid.NewGuid().ToString(), ManualName = "Manual 1" },
            new ManualDisplayViewModel { ManualId = Guid.NewGuid().ToString(), ManualName = "Manual 2" }
        };
            _manualServiceMock.Setup(service => service.GetListOfViewModelsAsync()).ReturnsAsync(manuals);

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(manuals, viewResult.Model);
        }

        [Test]
        public async Task Create_Get_ReturnsViewWithModel_WhenUrlIsSafe()
        {
            // Arrange
            var model = new ManualCreateViewModel();
            _manualServiceMock.Setup(service => service.GetCreateViewModelAsync(It.IsAny<string>())).ReturnsAsync(model);

            // Act
            var result = await _controller.Create("/safe/url");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task Create_Post_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            var model = new ManualCreateViewModel();
            _controller.ModelState.AddModelError("ManualName", "Required");

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task Create_Post_RedirectsToWrongData_WhenDataIsInvalid()
        {
            // Arrange
            var model = new ManualCreateViewModel
            {
                ManualName = "",
                MakerId = Guid.NewGuid().ToString(),
                EquipmentId = Guid.NewGuid().ToString()
            };

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("WrongData", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Upload_Post_ReturnsRedirectToUpload_WhenFileIsInvalid()
        {
            // Arrange
            IFormFile invalidFile = null;

            // Act
            var result = await _controller.Upload(invalidFile);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(ManualController.Upload), redirectResult.ActionName);
        }

        [Test]
        public async Task Upload_Post_ReturnsRedirectToWrongData_WhenFileTypeIsNotSupported()
        {
            // Arrange
            var invalidFile = new FormFile(new MemoryStream(), 0, 100, "Data", "test.txt");

            // Act
            var result = await _controller.Upload(invalidFile);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(ManualController.Upload), redirectResult.ActionName);
        }

        [Test]
        public async Task Delete_Get_ReturnsRedirectToWrongData_WhenIdIsInvalid()
        {
            // Act
            var result = await _controller.Delete("invalid-guid");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("WrongData", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Delete_Get_ReturnsViewWithModel_WhenValidId()
        {
            // Arrange
            var model = new ManualDeleteViewModel { ManualId = Guid.NewGuid().ToString(), ManualName = "Test Manual" };
            _manualServiceMock.Setup(service => service.GetItemToDeleteAsync(It.IsAny<string>())).ReturnsAsync(model);

            // Act
            var result = await _controller.Delete(model.ManualId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task Delete_Post_RedirectsToSelect_WhenSuccessful()
        {
            // Arrange
            var model = new ManualDeleteViewModel { ManualId = Guid.NewGuid().ToString() };
            _manualServiceMock.Setup(service => service.ConfirmDeleteAsync(model)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(ManualController.Select), redirectResult.ActionName);
        }
    }

}
