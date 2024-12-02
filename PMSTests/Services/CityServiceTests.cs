using MockQueryable;
using Moq;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data;
using PMSWeb.ViewModels.CityVM;

namespace PMSTests.Services
{
    [TestFixture]
    public class CityServiceTests
    {
        private Mock<IRepository<City, Guid>> _mockRepository;
        private CityService _cityService;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IRepository<City, Guid>>();
            _cityService = new CityService(_mockRepository.Object);
        }

        #region CreateCityAsync Tests

        [Test]
        public async Task CreateCityAsync_WithNullModel_ReturnsFalse()
        {
            // Act
            var result = await _cityService.CreateCityAsync(null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateCityAsync_WithNullName_ReturnsFalse()
        {
            // Arrange
            var model = new CityCreateViewModel { Name = null };

            // Act
            var result = await _cityService.CreateCityAsync(model);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateCityAsync_WithValidModel_CallsAddAsyncAndReturnsTrue()
        {
            // Arrange
            var model = new CityCreateViewModel { Name = "TestCity" };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<City>())).ReturnsAsync(true);

            // Act
            var result = await _cityService.CreateCityAsync(model);

            // Assert
            Assert.IsTrue(result);
            _mockRepository.Verify(r => r.AddAsync(It.Is<City>(c => c.Name == "TestCity")), Times.Once);
        }

        [Test]
        public async Task CreateCityAsync_WithRepositoryException_ReturnsFalse()
        {
            // Arrange
            var model = new CityCreateViewModel { Name = "TestCity" };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<City>())).ThrowsAsync(new Exception());

            // Act
            var result = await _cityService.CreateCityAsync(model);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region DeleteCityModelAsync Tests

        [Test]
        public async Task DeleteCityModelAsync_WithNullModel_ReturnsFalse()
        {
            // Act
            var result = await _cityService.DeleteCityModelAsync(null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteCityModelAsync_WithInvalidCityId_ReturnsFalse()
        {
            // Arrange
            var deleteModel = new CityDeleteViewModel { CityId = "invalid-guid" };
            var cities = new List<City>();
            _mockRepository.Setup(r => r.GetAllAsQueryable()).Returns(cities.AsQueryable().BuildMock());

            // Act
            var result = await _cityService.DeleteCityModelAsync(deleteModel);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteCityModelAsync_WithValidCityId_CallsDeleteByIdAsyncAndReturnsTrue()
        {
            // Arrange
            var cityId = Guid.NewGuid();
            var deleteModel = new CityDeleteViewModel { CityId = cityId.ToString() };
            var cities = new List<City> { new City { CityId = cityId, Name = "TestCity" } };
            _mockRepository.Setup(r => r.GetAllAsQueryable()).Returns(cities.AsQueryable().BuildMock());
            _mockRepository.Setup(r => r.DeleteByIdAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            // Act
            var result = await _cityService.DeleteCityModelAsync(deleteModel);

            // Assert
            Assert.IsTrue(result);
            _mockRepository.Verify(r => r.DeleteByIdAsync(cityId), Times.Once);
        }

        [Test]
        public async Task DeleteCityModelAsync_WithRepositoryException_ReturnsFalse()
        {
            // Arrange
            var cityId = Guid.NewGuid();
            var deleteModel = new CityDeleteViewModel { CityId = cityId.ToString() };
            var cities = new List<City> { new City { CityId = cityId, Name = "TestCity" } };
            _mockRepository.Setup(r => r.GetAllAsQueryable()).Returns(cities.AsQueryable().BuildMock());
            _mockRepository.Setup(r => r.DeleteByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception());

            // Act
            var result = await _cityService.DeleteCityModelAsync(deleteModel);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region GetListOfCitiesAsync Tests

        [Test]
        public async Task GetListOfCitiesAsync_WhenNoCitiesExist_ReturnsEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsQueryable()).Returns(new List<City>().AsQueryable().BuildMock());

            // Act
            var result = await _cityService.GetListOfCitiesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetListOfCitiesAsync_WithCities_ReturnsSortedCityList()
        {
            // Arrange
            var cities = new List<City>
            {
                new City { Name = "CityA", CreatedOn = DateTime.Now.AddDays(-1), CityId = Guid.NewGuid() },
                new City { Name = "CityB", CreatedOn = DateTime.Now, CityId = Guid.NewGuid() }
            };
            _mockRepository.Setup(r => r.GetAllAsQueryable()).Returns(cities.AsQueryable().BuildMock());

            // Act
            var result = await _cityService.GetListOfCitiesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("CityB", result.First().Name); // Sorted by CreatedOn descending
        }

        #endregion
    }
}
