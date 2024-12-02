using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMS.Services.Data.Interfaces;
using PMSWeb.Controllers;
using PMSWeb.ViewModels.Equipment;
using System.Security.Claims;

namespace PMSTests.Controllers
{
    [TestFixture]
    public class EquipmentControllerTests
    {
        private Mock<IEquipmentService> _equipmentServiceMock;
        private EquipmentController _controller;

        [SetUp]
        public void SetUp()
        {
            _equipmentServiceMock = new Mock<IEquipmentService>();
            _controller = new EquipmentController(_equipmentServiceMock.Object);

            // Mock user identity
            var userId = Guid.NewGuid().ToString();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId)
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
        public void Index_ReturnsView()
        {
            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Select_ReturnsRedirectToEmptyList_WhenNoEquipmentExists()
        {
            // Arrange
            _equipmentServiceMock.Setup(service => service.GetListOfViewModelsAsync())
                .ReturnsAsync(new List<EquipmentDisplayViewModel>());

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("EmptyList", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Select_ReturnsViewWithEquipmentList_WhenDataExists()
        {
            // Arrange
            var equipmentList = new List<EquipmentDisplayViewModel>
        {
            new EquipmentDisplayViewModel { EquipmentId = Guid.NewGuid().ToString(), Name = "Equipment 1" },
            new EquipmentDisplayViewModel { EquipmentId = Guid.NewGuid().ToString(), Name = "Equipment 2" }
        };
            _equipmentServiceMock.Setup(service => service.GetListOfViewModelsAsync()).ReturnsAsync(equipmentList);

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(equipmentList, viewResult.Model);
        }

        [Test]
        public async Task Create_Get_ReturnsViewWithModel()
        {
            // Arrange
            var model = new EquipmentCreateViewModel();
            _equipmentServiceMock.Setup(service => service.GetCreateModelAsync()).ReturnsAsync(model);

            // Act
            var result = await _controller.Create();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task Create_Get_RedirectsToNotFound_WhenModelIsNull()
        {
            // Arrange
            _equipmentServiceMock.Setup(service => service.GetCreateModelAsync()).ReturnsAsync((EquipmentCreateViewModel)null);

            // Act
            var result = await _controller.Create();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("NotFound", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Create_Post_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            var model = new EquipmentCreateViewModel();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(model, new List<Guid>(), new List<Guid>());

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task Create_Post_RedirectsToWrongData_WhenModelValidationFails()
        {
            // Arrange
            var model = new EquipmentCreateViewModel { Name = null };
            _equipmentServiceMock.Setup(service => service.CreateEquipmentAsync(It.IsAny<EquipmentCreateViewModel>(), It.IsAny<string>(), It.IsAny<List<Guid>>(), It.IsAny<List<Guid>>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Create(model, new List<Guid>(), new List<Guid>());

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
            var model = new EquipmentCreateViewModel
            {
                Name = "Test Equipment",
                Description = "Test Description",
                MakerId = Guid.NewGuid() // Valid GUID
            };

            // Mocking user identity
            var userId = Guid.NewGuid().ToString();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId)
    }));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Mock the service to simulate successful creation
            _equipmentServiceMock
                .Setup(service => service.CreateEquipmentAsync(
                    It.IsAny<EquipmentCreateViewModel>(),
                    userId,
                    It.IsAny<List<Guid>>(),
                    It.IsAny<List<Guid>>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Create(model, new List<Guid>(), new List<Guid>());

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(EquipmentController.Select), redirectResult.ActionName);
        }


        [Test]
        public async Task Edit_Get_ReturnsViewWithModel_WhenValidId()
        {
            // Arrange
            var model = new EquipmentEditViewModel { EquipmentId = Guid.NewGuid().ToString(), Name = "Equipment" };
            _equipmentServiceMock.Setup(service => service.GetItemForEditAsync(It.IsAny<string>())).ReturnsAsync(model);

            // Act
            var result = await _controller.Edit(model.EquipmentId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task Edit_Get_RedirectsToNotFound_WhenIdIsInvalid()
        {
            // Act
            var result = await _controller.Edit("invalid-guid");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("WrongData", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }
    }
}
