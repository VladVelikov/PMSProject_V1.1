using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PMS.Services.Data.Interfaces;
using PMSWeb.Controllers;
using PMSWeb.ViewModels.RM;

namespace PMSTests.Controllers
{
    [TestFixture]
    public class RMControllerTests
    {
        private Mock<IRMService> _rmServiceMock;
        private RMController _controller;

        [SetUp]
        public void SetUp()
        {
            _rmServiceMock = new Mock<IRMService>();
            _controller = new RMController(_rmServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test]
        public async Task Select_ReturnsRedirectToEmptyList_WhenNoItemsExist()
        {
            // Arrange
            _rmServiceMock.Setup(service => service.GetListOfViewModelsAsync()).ReturnsAsync((List<RMDisplayViewModel>)null);

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("EmptyList", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Select_ReturnsViewWithItems_WhenItemsExist()
        {
            // Arrange
            var items = new List<RMDisplayViewModel>
        {
            new RMDisplayViewModel { RoutMaintId = Guid.NewGuid().ToString(), Name = "Test RM" }
        };
            _rmServiceMock.Setup(service => service.GetListOfViewModelsAsync()).ReturnsAsync(items);

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(items, viewResult.Model);
        }

        [Test]
        public async Task Create_Post_RedirectsToWrongData_WhenUserIdIsInvalid()
        {
            // Arrange
            var model = new RMCreateViewModel
            {
                ResponsiblePosition = "Manager", // Ensure valid position
                Name = "Test RM",
                Interval = 30
            };

            // Do not set any user context to simulate an invalid user ID scenario
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() // No user is set here
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
        public async Task Create_Post_ReturnsViewWithModel_WhenModelStateIsInvalid()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var model = new RMCreateViewModel
            {
                ResponsiblePosition = "Manager", // Valid position
                Name = "", // Invalid Name to cause model state validation failure
                Interval = 30
            };

            // Mock user context
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId)
    }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Add model state error to simulate validation failure
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult?.Model); // Ensure the same model is returned
        }


        [Test]
        public async Task Create_Post_RedirectsToSelect_WhenCreationSucceeds()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var model = new RMCreateViewModel
            {
                ResponsiblePosition = "Manager", // Ensure this matches one of the valid positions
                Name = "Test RM",
                Interval = 30
            };

            // Mock the service to return true for successful creation
            _rmServiceMock.Setup(service => service.CreateRMAsync(It.IsAny<RMCreateViewModel>(), It.IsAny<string>()))
                          .ReturnsAsync(true);

            // Set up the mock user context
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId)
    }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(RMController.Select), redirectResult.ActionName);
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
        public async Task Edit_Get_ReturnsViewWithModel_WhenRMExists()
        {
            // Arrange
            var model = new RMEditViewModel { RMId = Guid.NewGuid().ToString(), Name = "Test RM" };
            _rmServiceMock.Setup(service => service.GetItemForEditAsync(It.IsAny<string>())).ReturnsAsync(model);

            // Act
            var result = await _controller.Edit(model.RMId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task Delete_Get_ReturnsRedirectToNotFound_WhenRMDoesNotExist()
        {
            // Arrange
            _rmServiceMock.Setup(service => service.GetItemToDeleteAsync(It.IsAny<string>()))
                          .ReturnsAsync((RMDeleteViewModel)null);

            // Act
            var result = await _controller.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("NotFound", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Delete_Post_RedirectsToSelect_WhenSuccessful()
        {
            // Arrange
            var model = new RMDeleteViewModel { RmId = Guid.NewGuid().ToString() };
            _rmServiceMock.Setup(service => service.ConfirmDeleteAsync(It.IsAny<RMDeleteViewModel>())).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(RMController.Select), redirectResult.ActionName);
        }

        [Test]
        public async Task Details_ReturnsRedirectToNotFound_WhenRMDoesNotExist()
        {
            // Arrange
            _rmServiceMock.Setup(service => service.GetDetailsAsync(It.IsAny<string>()))
                          .ReturnsAsync((RMDetailsViewModel)null);

            // Act
            var result = await _controller.Details(Guid.NewGuid().ToString());

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("NotFound", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Details_ReturnsViewWithModel_WhenRMExists()
        {
            // Arrange
            var model = new RMDetailsViewModel { RoutMaintId = Guid.NewGuid().ToString() };
            _rmServiceMock.Setup(service => service.GetDetailsAsync(It.IsAny<string>())).ReturnsAsync(model);

            // Act
            var result = await _controller.Details(model.RoutMaintId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }
    }

}
