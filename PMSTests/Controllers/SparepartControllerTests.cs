using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMS.Services.Data.Interfaces;
using PMSWeb.Controllers;
using PMSWeb.ViewModels.SparepartVM;
using System.Security.Claims;

namespace PMSTests.Controllers
{
    [TestFixture]
    public class SparepartControllerTests : IDisposable
    {
        private Mock<ISparepartService> _sparesServiceMock;
        private SparepartController _controller;

        [SetUp]
        public void SetUp()
        {
            _sparesServiceMock = new Mock<ISparepartService>();
            _controller = new SparepartController(_sparesServiceMock.Object);

            // Mock the User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
    }));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [TearDown]
        public void TearDown()
        {
            _sparesServiceMock = null;
            _controller = null;
        }

        [Test]
        public async Task Select_ReturnsViewWithSparesList_WhenSparesExist()
        {
            // Arrange
            var sparesList = new List<SparepartDisplayViewModel>
        {
            new SparepartDisplayViewModel { SparepartId = "1", Name = "Spare 1" }
        };
            _sparesServiceMock.Setup(s => s.GetListOfViewModelsAsync()).ReturnsAsync(sparesList);

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult.Model);
            Assert.AreEqual(sparesList, viewResult.Model);
        }

        [Test]
        public async Task Select_RedirectsToEmptyList_WhenNoSparesExist()
        {
            // Arrange
            _sparesServiceMock.Setup(s => s.GetListOfViewModelsAsync()).ReturnsAsync((List<SparepartDisplayViewModel>)null);

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
            var model = new SparepartCreateViewModel();
            _sparesServiceMock.Setup(s => s.GetItemForCreateAsync()).ReturnsAsync(model);

            // Act
            var result = await _controller.Create();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task Create_Get_RedirectsToWrongData_WhenModelIsNull()
        {
            // Arrange
            _sparesServiceMock.Setup(s => s.GetItemForCreateAsync()).ReturnsAsync((SparepartCreateViewModel)null);

            // Act
            var result = await _controller.Create();

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
            var model = new SparepartCreateViewModel();
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task Create_Post_RedirectsToWrongData_WhenModelIsInvalid()
        {
            // Arrange
            var model = new SparepartCreateViewModel
            {
                Name = "Spare",
                ROB = -1 // Invalid ROB
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
        public async Task Create_Post_RedirectsToSelect_WhenCreationSucceeds()
        {
            // Arrange
            var model = new SparepartCreateViewModel
            {
                Name = "Test Sparepart",
                ROB = 10,
                Price = 100.50m,
                Units = "Pieces",
                EquipmentId = Guid.NewGuid().ToString()
            };
            _sparesServiceMock.Setup(service => service.CreateSparepartAsync(model, It.IsAny<string>()))
                              .ReturnsAsync(true);

            // Act
            var result = await _controller.Create(model) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(_controller.Select), result.ActionName);
        }


        [Test]
        public async Task Details_ReturnsViewWithModel_WhenModelIsValid()
        {
            // Arrange
            var validId = Guid.NewGuid().ToString();
            var validModel = new SparepartDetailsViewModel
            {
                SparepartId = validId,
                Name = "Valid Sparepart",
                Price = "100.00m",
                ROB = "10.5"
            };

            _sparesServiceMock.Setup(service => service.GetDetailsAsync(validId))
                              .ReturnsAsync(validModel);

            // Act
            var result = await _controller.Details(validId) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            var model = result.Model as SparepartDetailsViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(validId, model.SparepartId);
            Assert.AreEqual("Valid Sparepart", model.Name);
            Assert.AreEqual("100.00m", model.Price);
            Assert.AreEqual("10.5", model.ROB);
        }


        [Test]
        public async Task Details_RedirectsToNotFound_WhenModelIsNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid().ToString();
            _sparesServiceMock.Setup(service => service.GetDetailsAsync(invalidId))
                              .ReturnsAsync((SparepartDetailsViewModel)null);

            // Act
            var result = await _controller.Details(invalidId) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("NotFound", result.ActionName);
            Assert.AreEqual("Crushes", result.ControllerName);
        }


        public void Dispose()
        {
            _controller?.Dispose();
        }

        // Add similar tests for Edit, Delete, etc.
    }

}
