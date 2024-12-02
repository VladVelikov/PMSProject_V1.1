using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PMS.Services.Data.Interfaces;
using PMSWeb.Controllers;
using PMSWeb.ViewModels.CityVM;

namespace PMSTests.Controllers
{
    [TestFixture]
    public class CityControllerTests : IDisposable
    {
        private Mock<ICityService>? _cityServiceMock;
        private CityController? _controller;

        [SetUp]
        public void SetUp()
        {
            _cityServiceMock = new Mock<ICityService>();
            _controller = new CityController(_cityServiceMock.Object);
        }
        
        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test]
        public async Task Select_ReturnsViewWithListOfCities()
        {
            // Arrange
            var cities = new List<CityDisplayViewModel>
        {
            new CityDisplayViewModel { CityId = Guid.NewGuid().ToString(), Name = "City 1" },
            new CityDisplayViewModel { CityId = Guid.NewGuid().ToString(), Name = "City 2" }
        };

            _cityServiceMock.Setup(service => service.GetListOfCitiesAsync()).ReturnsAsync(cities);

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(cities, viewResult.Model);
        }

        [Test]
        public void Create_Get_ReturnsEmptyCreateViewModel()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<CityCreateViewModel>(viewResult.Model);
        }

        [Test]
        public async Task Create_Post_InvalidModel_RedirectsToModelNotValid()
        {
            // Arrange
            var model = new CityCreateViewModel();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("ModelNotValid", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Create_Post_WhenServiceFails_RedirectsToNotCreated()
        {
            // Arrange
            var model = new CityCreateViewModel { Name = "Test City" };
            _cityServiceMock.Setup(service => service.CreateCityAsync(model)).ReturnsAsync(false);

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
            var model = new CityCreateViewModel { Name = "Test City" };
            _cityServiceMock.Setup(service => service.CreateCityAsync(model)).ReturnsAsync(true);

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(CityController.Select), redirectResult.ActionName);
        }

        [Test]
        public async Task Delete_Get_InvalidId_RedirectsToNotDeleted()
        {
            // Act
            var result = await _controller.Delete("invalid-guid");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("NotDeleted", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Delete_Get_WhenServiceReturnsInvalidModel_RedirectsToNotDeleted()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            _cityServiceMock.Setup(service => service.GetDeleteCityModelAsync(id))
                            .ReturnsAsync(new CityDeleteViewModel { CityId = string.Empty });

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("NotDeleted", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Delete_Get_WhenValidId_ReturnsDeleteView()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var deleteModel = new CityDeleteViewModel { CityId = id, Name = "Test City" };
            _cityServiceMock.Setup(service => service.GetDeleteCityModelAsync(id)).ReturnsAsync(deleteModel);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(deleteModel, viewResult.Model);
        }

        [Test]
        public async Task Delete_Post_InvalidModel_RedirectsToModelNotFound()
        {
            // Arrange
            var model = new CityDeleteViewModel();
            _controller.ModelState.AddModelError("CityId", "Required");

            // Act
            var result = await _controller.Delete(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("ModelNotFound", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Delete_Post_WhenServiceFails_RedirectsToNotDeleted()
        {
            // Arrange
            var model = new CityDeleteViewModel { CityId = Guid.NewGuid().ToString() };
            _cityServiceMock.Setup(service => service.DeleteCityModelAsync(model)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("NotDeleted", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Delete_Post_WhenSuccessful_RedirectsToSelect()
        {
            // Arrange
            var model = new CityDeleteViewModel { CityId = Guid.NewGuid().ToString() };
            _cityServiceMock.Setup(service => service.DeleteCityModelAsync(model)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(CityController.Select), redirectResult.ActionName);
        }

        public void Dispose()
        {
            _controller?.Dispose();  
        }
    }
}
