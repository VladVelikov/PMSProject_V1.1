using MockQueryable;
using Moq;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data;
using PMSWeb.ViewModels.CountryVM;

namespace PMSTests.Services
{
    [TestFixture]
    public class CountryServiceTests
    {
        private Mock<IRepository<Country, Guid>> _mockRepository;
        private CountryService _countryService;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IRepository<Country, Guid>>();
            _countryService = new CountryService(_mockRepository.Object);
        }

        #region CreateCountryAsync Tests

        [Test]
        public async Task CreateCountryAsync_WithNullModel_ReturnsFalse()
        {
            // Act
            var result = await _countryService.CreateCountryAsync(null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateCountryAsync_WithNullName_ReturnsFalse()
        {
            // Arrange
            var model = new CountryCreateViewModel { Name = null };

            // Act
            var result = await _countryService.CreateCountryAsync(model);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateCountryAsync_WithValidModel_CallsAddAsyncAndReturnsTrue()
        {
            // Arrange
            var model = new CountryCreateViewModel { Name = "TestCountry" };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Country>())).ReturnsAsync(true);

            // Act
            var result = await _countryService.CreateCountryAsync(model);

            // Assert
            Assert.IsTrue(result);
            _mockRepository.Verify(r => r.AddAsync(It.Is<Country>(c => c.Name == "TestCountry")), Times.Once);
        }

        [Test]
        public async Task CreateCountryAsync_WithRepositoryException_ReturnsFalse()
        {
            // Arrange
            var model = new CountryCreateViewModel { Name = "TestCountry" };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Country>())).ThrowsAsync(new Exception());

            // Act
            var result = await _countryService.CreateCountryAsync(model);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region DeleteCountryModelAsync Tests

        [Test]
        public async Task DeleteCountryModelAsync_WithNullModel_ReturnsFalse()
        {
            // Act
            var result = await _countryService.DeleteCountryModelAsync(null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteCountryModelAsync_WithInvalidCountryId_ReturnsFalse()
        {
            // Arrange
            var deleteModel = new CountryDeleteViewModel { CountryId = "invalid-guid" };
            _mockRepository.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<Country>().AsQueryable().BuildMock());

            // Act
            var result = await _countryService.DeleteCountryModelAsync(deleteModel);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteCountryModelAsync_WithValidCountryId_CallsDeleteByIdAsyncAndReturnsTrue()
        {
            // Arrange
            var countryId = Guid.NewGuid();
            var deleteModel = new CountryDeleteViewModel { CountryId = countryId.ToString() };
            var countries = new List<Country> { new Country { CountryId = countryId, Name = "TestCountry" } };
            _mockRepository.Setup(r => r.GetAllAsQueryable()).Returns(countries.AsQueryable().BuildMock());
            _mockRepository.Setup(r => r.DeleteByIdAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            // Act
            var result = await _countryService.DeleteCountryModelAsync(deleteModel);

            // Assert
            Assert.IsTrue(result);
            _mockRepository.Verify(r => r.DeleteByIdAsync(countryId), Times.Once);
        }

        [Test]
        public async Task DeleteCountryModelAsync_WithRepositoryException_ReturnsFalse()
        {
            // Arrange
            var countryId = Guid.NewGuid();
            var deleteModel = new CountryDeleteViewModel { CountryId = countryId.ToString() };
            var countries = new List<Country> { new Country { CountryId = countryId, Name = "TestCountry" } };
            _mockRepository.Setup(r => r.GetAllAsQueryable()).Returns(countries.AsQueryable().BuildMock());
            _mockRepository.Setup(r => r.DeleteByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception());

            // Act
            var result = await _countryService.DeleteCountryModelAsync(deleteModel);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region GetDeleteCountryModelAsync Tests

        [Test]
        public async Task GetDeleteCountryModelAsync_WithValidCountryId_ReturnsModel()
        {
            // Arrange
            var countryId = Guid.NewGuid();
            var countries = new List<Country> { new Country { CountryId = countryId, Name = "TestCountry", CreatedOn = DateTime.Now } };
            _mockRepository.Setup(r => r.GetAllAsQueryable()).Returns(countries.AsQueryable().BuildMock());

            // Act
            var result = await _countryService.GetDeleteCountryModelAsync(countryId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(countryId.ToString(), result.CountryId);
            Assert.AreEqual("TestCountry", result.Name);
        }

        [Test]
        public async Task GetDeleteCountryModelAsync_WithInvalidCountryId_ReturnsEmptyModel()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsQueryable()).Returns(new List<Country>().AsQueryable().BuildMock());

            // Act
            var result = await _countryService.GetDeleteCountryModelAsync(Guid.NewGuid().ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Name);
        }

        #endregion

        #region GetListOfCountriesAsync Tests

        [Test]
        public async Task GetListOfCountriesAsync_WhenNoCountriesExist_ReturnsEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsQueryable()).Returns(new List<Country>().AsQueryable().BuildMock());

            // Act
            var result = await _countryService.GetListOfCountriesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetListOfCountriesAsync_WithCountries_ReturnsSortedCountryList()
        {
            // Arrange
            var countries = new List<Country>
            {
                new Country { Name = "CountryA", CreatedOn = DateTime.Now.AddDays(-1), CountryId = Guid.NewGuid() },
                new Country { Name = "CountryB", CreatedOn = DateTime.Now, CountryId = Guid.NewGuid() }
            };
            _mockRepository.Setup(r => r.GetAllAsQueryable()).Returns(countries.AsQueryable().BuildMock());

            // Act
            var result = await _countryService.GetListOfCountriesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("CountryB", result.First().Name); // Sorted by CreatedOn descending
        }

        #endregion
    }
}
