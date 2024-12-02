using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMS.Services.Data.Interfaces;
using PMSWeb.Controllers;
using PMSWeb.ViewModels.Maker;
using System.Security.Claims;

namespace PMSTests.Controllers
{
    [TestFixture]
    public class MakerControllerTests
    {
        private Mock<IMakerService> _makerServiceMock;
        private MakerController _controller;

        [SetUp]
        public void SetUp()
        {
            _makerServiceMock = new Mock<IMakerService>();
            _controller = new MakerController(_makerServiceMock.Object);

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
        public async Task Select_ReturnsViewWithMakers()
        {
            // Arrange
            var makers = new List<MakerDisplayViewModel>
        {
            new MakerDisplayViewModel { MakerId = Guid.NewGuid().ToString(), Name = "Maker 1" },
            new MakerDisplayViewModel { MakerId = Guid.NewGuid().ToString(), Name = "Maker 2" }
        };
            _makerServiceMock.Setup(service => service.GetListOfViewModelsAsync()).ReturnsAsync(makers);

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(makers, viewResult.Model);
        }

        [Test]
        public void Create_Get_ReturnsViewWithEmptyModel()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<MakerCreateViewModel>(viewResult.Model);
        }

        [Test]
        public async Task Create_Post_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            var model = new MakerCreateViewModel();
            _controller.ModelState.AddModelError("MakerName", "Required");

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task Create_Post_RedirectsToWrongData_WhenUserIdIsInvalid()
        {
            // Arrange
            var model = new MakerCreateViewModel { MakerName = "Test Maker" };
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(); // No user ID

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("WrongData", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Create_Post_RedirectsToSelect_WhenSuccessful()
        {
            // Arrange
            var model = new MakerCreateViewModel { MakerName = "Test Maker" };
            _makerServiceMock.Setup(service => service.CreateMakerAsync(model, It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(MakerController.Select), redirectResult.ActionName);
        }

        [Test]
        public async Task Edit_Get_ReturnsRedirectToWrongData_WhenIdIsInvalid()
        {
            // Act
            var result = await _controller.Edit("invalid-guid");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("WrongData", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Edit_Get_ReturnsViewWithModel_WhenValidId()
        {
            // Arrange
            var model = new MakerEditViewModel { MakerId = Guid.NewGuid().ToString(), MakerName = "Test Maker" };
            _makerServiceMock.Setup(service => service.GetItemForEditAsync(It.IsAny<string>())).ReturnsAsync(model);

            // Act
            var result = await _controller.Edit(model.MakerId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
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
            var model = new MakerDeleteViewModel { MakerId = Guid.NewGuid().ToString(), Name = "Test Maker" };
            _makerServiceMock.Setup(service => service.GetItemToDeleteAsync(It.IsAny<string>())).ReturnsAsync(model);

            // Act
            var result = await _controller.Delete(model.MakerId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task Delete_Post_RedirectsToSelect_WhenSuccessful()
        {
            // Arrange
            var model = new MakerDeleteViewModel { MakerId = Guid.NewGuid().ToString() };
            _makerServiceMock.Setup(service => service.ConfirmDeleteAsync(model)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(MakerController.Select), redirectResult.ActionName);
        }
    }

}
