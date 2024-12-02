using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMS.Services.Data.Interfaces;
using PMSWeb.Controllers;
using PMSWeb.ViewModels.SM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PMSTests.Controllers
{
    [TestFixture]
    public class SMControllerTests : IDisposable
    {
        private Mock<ISMService> _smServiceMock;
        private SMController _controller;

        [SetUp]
        public void SetUp()
        {
            _smServiceMock = new Mock<ISMService>();
            _controller = new SMController(_smServiceMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose of the controller instance if need to do so
            if (_controller != null)
            {
                _controller.Dispose();
            }

            // Reset the mocks... compulsory
            _smServiceMock.Reset();
        }


        [Test]
        public async Task Select_ReturnsViewWithModel_WhenListIsNotEmpty()
        {
            // Arrange
            var smList = new List<SMDisplayViewModel>
        {
            new SMDisplayViewModel { SpecMaintId = "1", Name = "Test SM 1" },
            new SMDisplayViewModel { SpecMaintId = "2", Name = "Test SM 2" }
        };
            _smServiceMock.Setup(s => s.GetListOfViewModelsAsync()).ReturnsAsync(smList);

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(smList, viewResult.Model);
        }

        [Test]
        public async Task Select_RedirectsToEmptyList_WhenListIsNull()
        {
            // Arrange
            _smServiceMock.Setup(s => s.GetListOfViewModelsAsync()).ReturnsAsync((List<SMDisplayViewModel>)null);

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("EmptyList", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Create_Get_ReturnsViewWithModel_WhenModelIsValid()
        {
            // Arrange
            var createModel = new SMCreateViewModel();
            _smServiceMock.Setup(s => s.GetItemForCreateAsync()).ReturnsAsync(createModel);

            // Act
            var result = await _controller.Create();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(createModel, viewResult.Model);
        }

        [Test]
        public async Task Create_Get_RedirectsToWrongData_WhenModelIsNull()
        {
            // Arrange
            _smServiceMock.Setup(s => s.GetItemForCreateAsync()).ReturnsAsync((SMCreateViewModel)null);

            // Act
            var result = await _controller.Create();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("WrongData", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Create_Post_RedirectsToSelect_WhenCreationSucceeds()
        {
            // Arrange
            var model = new SMCreateViewModel
            {
                ResponsiblePosition = "Manager",
                Name = "Test SM",
                Interval = 30
            };
            _smServiceMock.Setup(s => s.CreateSpecificMaintenanceAsync(model, It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(SMController.Select), redirectResult.ActionName);
        }

        [Test]
        public async Task Create_Post_RedirectsToNotCreated_WhenCreationFails()
        {
            // Arrange
            var model = new SMCreateViewModel
            {
                ResponsiblePosition = "Manager",
                Name = "Test SM",
                Interval = 30
            };
            _smServiceMock.Setup(s => s.CreateSpecificMaintenanceAsync(model, It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("NotCreated", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Edit_Get_ReturnsViewWithModel_WhenModelIsValid()
        {
            // Arrange
            var validId = Guid.NewGuid().ToString();
            var validModel = new SMEditViewModel
            {
                SMId = validId,
                Name = "Specific Maintenance",
                Interval = 30,
                ResponsiblePosition = "Manager"
            };

            _smServiceMock.Setup(s => s.GetItemForEditAsync(validId)).ReturnsAsync(validModel);

            // Act
            var result = await _controller.Edit(validId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            Assert.AreEqual(validModel, viewResult.Model);
        }



        [Test]
        public async Task Edit_Get_RedirectsToNotFound_WhenModelIsNull()
        {
            // Arrange
            var validId = Guid.NewGuid().ToString();
            _smServiceMock.Setup(s => s.GetItemForEditAsync(validId)).ReturnsAsync((SMEditViewModel)null);

            // Act
            var result = await _controller.Edit(validId);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("NotFound", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }


        [Test]
        public async Task Delete_Post_RedirectsToSelect_WhenDeletionSucceeds()
        {
            // Arrange
            var model = new SMDeleteViewModel { SmId = "1" };
            _smServiceMock.Setup(s => s.ConfirmDeleteAsync(model)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(SMController.Select), redirectResult.ActionName);
        }

        [Test]
        public async Task Delete_Post_RedirectsToNotDeleted_WhenDeletionFails()
        {
            // Arrange
            var model = new SMDeleteViewModel { SmId = "1" };
            _smServiceMock.Setup(s => s.ConfirmDeleteAsync(model)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("NotDeleted", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Details_ReturnsViewWithModel_WhenModelIsValid()
        {
            // Arrange
            var validId = Guid.NewGuid().ToString();
            var validModel = new SMDetailsViewModel
            {
                SpecMaintId = validId,
                Name = "Valid Specific Maintenance"
            };

            _smServiceMock.Setup(s => s.GetDetailsAsync(validId)).ReturnsAsync(validModel);

            // Act
            var result = await _controller.Details(validId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            Assert.AreEqual(validModel, viewResult.Model);
        }


        [Test]
        public async Task Details_RedirectsToNotFound_WhenModelIsNull()
        {
            // Arrange
            var validId = Guid.NewGuid().ToString();
            _smServiceMock.Setup(s => s.GetDetailsAsync(validId)).ReturnsAsync((SMDetailsViewModel)null);

            // Act
            var result = await _controller.Details(validId);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("NotFound", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }
        [Test]
        public async Task Details_RedirectsToWrongData_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = "invalid-guid";

            // Act
            var result = await _controller.Details(invalidId);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("WrongData", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        public void Dispose()
        {
            _controller?.Dispose();  
        }
    }

}
