using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PMS.Services.Data.Interfaces;
using PMSWeb.Controllers;
using PMSWeb.ViewModels.CountryVM;

namespace PMSTests.Controllers
{
    [TestFixture]
    public class CountryControllerTests
    {
        private Mock<ICountryService> _countryServiceMock;
        private CountryController _controller;

        [SetUp]
        public void SetUp()
        {
            _countryServiceMock = new Mock<ICountryService>();
            _controller = new CountryController(_countryServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test]
        public async Task Select_ReturnsRedirectToEmptyList_WhenNoCountriesExist()
        {
            // Arrange
            _countryServiceMock
                .Setup(service => service.GetListOfCountriesAsync())
                .ReturnsAsync(new List<CountryDisplayViewModel>());

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("EmptyList", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Select_ReturnsViewWithCountries_WhenDataExists()
        {
            // Arrange
            var countries = new[]
            {
            new CountryDisplayViewModel { CountryId = Guid.NewGuid().ToString(), Name = "Country 1" },
            new CountryDisplayViewModel { CountryId = Guid.NewGuid().ToString(), Name = "Country 2" }
        };

            _countryServiceMock.Setup(service => service.GetListOfCountriesAsync()).ReturnsAsync(countries);

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(countries, viewResult.Model);
        }

        [Test]
        public void Create_Get_ReturnsViewWithEmptyModel()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<CountryCreateViewModel>(viewResult.Model);
        }

        [Test]
        public async Task Create_Post_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            var model = new CountryCreateViewModel();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task Create_Post_WhenSuccessful_RedirectsToSelect()
        {
            // Arrange
            var model = new CountryCreateViewModel { Name = "Test Country" };
            _countryServiceMock.Setup(service => service.CreateCountryAsync(model)).ReturnsAsync(true);

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(CountryController.Select), redirectResult.ActionName);
        }

        [Test]
        public async Task Delete_Get_InvalidId_RedirectsToWrongData()
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
        public async Task Delete_Get_WhenNotFound_RedirectsToNotFound()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            _countryServiceMock.Setup(service => service.GetDeleteCountryModelAsync(id))
                .ReturnsAsync((CountryDeleteViewModel)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("NotFound", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Delete_Get_WhenModelIsValid_ReturnsViewWithModel()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var model = new CountryDeleteViewModel { CountryId = id, Name = "Test Country" };
            _countryServiceMock.Setup(service => service.GetDeleteCountryModelAsync(id)).ReturnsAsync(model);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task Delete_Post_InvalidModel_RedirectsToSelect()
        {
            // Arrange
            var model = new CountryDeleteViewModel();
            _controller.ModelState.AddModelError("CountryId", "Required");

            // Act
            var result = await _controller.Delete(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(CountryController.Select), redirectResult.ActionName);
        }

        [Test]
        public async Task Delete_Post_WhenNotDeleted_RedirectsToNotDeleted()
        {
            // Arrange
            var model = new CountryDeleteViewModel { CountryId = Guid.NewGuid().ToString() };
            _countryServiceMock.Setup(service => service.DeleteCountryModelAsync(model)).ReturnsAsync(false);

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
            var model = new CountryDeleteViewModel { CountryId = Guid.NewGuid().ToString() };
            _countryServiceMock.Setup(service => service.DeleteCountryModelAsync(model)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(CountryController.Select), redirectResult.ActionName);
        }
    }
}
