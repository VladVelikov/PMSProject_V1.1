using MockQueryable;
using Moq;
using PMS.Data.Models;
using PMS.Data.Models.Identity;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data;
using PMSWeb.ViewModels.SupplierVM;

namespace PMSTests.Services
{
    [TestFixture]
    public class SupplierServiceTests
    {
        private Mock<IRepository<Supplier, Guid>> _suppliersRepoMock;
        private Mock<IRepository<Country, Guid>> _countriesRepoMock;
        private Mock<IRepository<City, Guid>> _citiesRepoMock;
        private Mock<IRepository<Sparepart, Guid>> _sparesRepoMock;
        private Mock<IRepository<Consumable, Guid>> _consumablesRepoMock;
        private Mock<IRepository<SparepartSupplier, Guid[]>> _sparePartsSuppliersRepoMock;
        private Mock<IRepository<ConsumableSupplier, Guid[]>> _consumablesSuppliersRepoMock;
        private SupplierService _service;

        [SetUp]
        public void SetUp()
        {
            _suppliersRepoMock = new Mock<IRepository<Supplier, Guid>>();
            _countriesRepoMock = new Mock<IRepository<Country, Guid>>();
            _citiesRepoMock = new Mock<IRepository<City, Guid>>();
            _sparesRepoMock = new Mock<IRepository<Sparepart, Guid>>();
            _consumablesRepoMock = new Mock<IRepository<Consumable, Guid>>();
            _sparePartsSuppliersRepoMock = new Mock<IRepository<SparepartSupplier, Guid[]>>();
            _consumablesSuppliersRepoMock = new Mock<IRepository<ConsumableSupplier, Guid[]>>();

            _service = new SupplierService(
                _suppliersRepoMock.Object,
                _countriesRepoMock.Object,
                _citiesRepoMock.Object,
                _sparesRepoMock.Object,
                _consumablesRepoMock.Object,
                _sparePartsSuppliersRepoMock.Object,
                _consumablesSuppliersRepoMock.Object
            );
        }

        [Test]
        public async Task GetListOfViewModelsAsync_ShouldReturnModels_WhenDataExists()
        {
            var suppliers = new List<Supplier>
        {
            new Supplier
            {
                SupplierId = Guid.NewGuid(),
                Name = "Supplier 1",
                Address = "Address 1",
                Email = "email1@example.com",
                PhoneNumber = "123456789",
                City = new City { Name = "City 1" },
                Country = new Country { Name = "Country 1" },
                IsDleted = false,
                EditedOn = DateTime.UtcNow
            }
        }.AsQueryable().BuildMock();

            _suppliersRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(suppliers);

            var result = await _service.GetListOfViewModelsAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Supplier 1", result.First().Name);
        }

        [Test]
        public async Task GetListOfViewModelsAsync_ShouldReturnEmptyList_WhenNoData()
        {
            var suppliers = new List<Supplier>().AsQueryable().BuildMock();

            _suppliersRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(suppliers);

            var result = await _service.GetListOfViewModelsAsync();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetItemForCreateAsync_ShouldReturnPopulatedModel()
        {
            // Arrange
            var countries = new List<Country>
    {
        new Country { CountryId = Guid.NewGuid(), Name = "Country 1" }
    }.AsQueryable().BuildMock();

            var cities = new List<City>
    {
        new City { CityId = Guid.NewGuid(), Name = "City 1" }
    }.AsQueryable().BuildMock();

            var spareparts = new List<Sparepart>
    {
        new Sparepart { SparepartId = Guid.NewGuid(), SparepartName = "Sparepart 1", IsDeleted = false }
    }.AsQueryable().BuildMock();

            var consumables = new List<Consumable>
    {
        new Consumable { ConsumableId = Guid.NewGuid(), Name = "Consumable 1", IsDeleted = false }
    }.AsQueryable().BuildMock();

            _countriesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(countries);
            _citiesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(cities);
            _sparesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(spareparts);
            _consumablesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(consumables);

            // Act
            var result = await _service.GetItemForCreateAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Countries.Count);
            Assert.AreEqual("Country 1", result.Countries.First().Name);
            Assert.AreEqual(1, result.Cities.Count);
            Assert.AreEqual("City 1", result.Cities.First().Name);
            Assert.AreEqual(1, result.Spareparts.Count);
            Assert.AreEqual("Sparepart 1", result.Spareparts.First().Name);
            Assert.AreEqual(1, result.Consumables.Count);
            Assert.AreEqual("Consumable 1", result.Consumables.First().Name);
        }


        [Test]
        public async Task CreateSparepartAsync_ShouldReturnTrue_WhenValidData()
        {
            // Arrange
            var supplierId = Guid.NewGuid();
            var sparepartId = Guid.NewGuid();
            var consumableId = Guid.NewGuid();

            var suppliers = new List<Supplier>().AsQueryable().BuildMock();
            var sparePartsSuppliers = new List<SparepartSupplier>().AsQueryable().BuildMock();
            var consumablesSuppliers = new List<ConsumableSupplier>().AsQueryable().BuildMock();

            _suppliersRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(suppliers);
            _sparePartsSuppliersRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(sparePartsSuppliers);
            _consumablesSuppliersRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(consumablesSuppliers);
            _suppliersRepoMock.Setup(repo => repo.AddAsync(It.IsAny<Supplier>())).ReturnsAsync(true);
            _sparePartsSuppliersRepoMock.Setup(repo => repo.AddRangeAsync(It.IsAny<IEnumerable<SparepartSupplier>>()))
                                        .ReturnsAsync(true);
            _consumablesSuppliersRepoMock.Setup(repo => repo.AddRangeAsync(It.IsAny<IEnumerable<ConsumableSupplier>>()))
                                        .ReturnsAsync(true);

            var model = new SupplierCreateViewModel
            {
                Name = "Supplier 1",
                Address = "Address 1",
                Email = "email@example.com",
                PhoneNumber = "123456789",
                CityId = Guid.NewGuid().ToString(),
                CountryId = Guid.NewGuid().ToString()
            };

            var spareparts = new List<Guid> { sparepartId };
            var consumables = new List<Guid> { consumableId };

            // Act
            var result = await _service.CreateSparepartAsync(model, "test-user", spareparts, consumables);

            // Assert
            Assert.IsTrue(result);
        }


        [Test]
        public async Task GetItemForEditAsync_ShouldReturnModel_WhenItemExists()
        {
            // Arrange
            var supplierId = Guid.NewGuid();
            var countries = new List<Country>
    {
        new Country { CountryId = Guid.NewGuid(), Name = "Country 1" }
    }.AsQueryable().BuildMock();

            var cities = new List<City>
    {
        new City { CityId = Guid.NewGuid(), Name = "City 1" }
    }.AsQueryable().BuildMock();

            var suppliers = new List<Supplier>
    {
        new Supplier
        {
            SupplierId = supplierId,
            Name = "Supplier 1",
            Address = "Address 1",
            Email = "email@example.com",
            PhoneNumber = "123456789",
            CityId = Guid.NewGuid(),
            CountryId = Guid.NewGuid(),
            IsDleted = false
        }
    }.AsQueryable().BuildMock();

            var sparepartsSuppliers = new List<SparepartSupplier>().AsQueryable().BuildMock();
            var consumablesSuppliers = new List<ConsumableSupplier>().AsQueryable().BuildMock();
            var spareparts = new List<Sparepart>().AsQueryable().BuildMock();
            var consumables = new List<Consumable>().AsQueryable().BuildMock();

            _countriesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(countries);
            _citiesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(cities);
            _suppliersRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(suppliers);
            _sparePartsSuppliersRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(sparepartsSuppliers);
            _consumablesSuppliersRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(consumablesSuppliers);
            _sparesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(spareparts);
            _consumablesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(consumables);

            // Act
            var result = await _service.GetItemForEditAsync(supplierId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Supplier 1", result.Name);
            Assert.AreEqual("Address 1", result.Address);
            Assert.AreEqual("email@example.com", result.Email);
            Assert.AreEqual("123456789", result.PhoneNumber);
        }


        [Test]
        public async Task ConfirmDeleteAsync_ShouldReturnTrue_WhenDeleteSucceeds()
        {
            var supplierId = Guid.NewGuid();
            var suppliers = new List<Supplier>
        {
            new Supplier
            {
                SupplierId = supplierId,
                IsDleted = false
            }
        }.AsQueryable().BuildMock();

            _suppliersRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(suppliers);
            _suppliersRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Supplier>())).ReturnsAsync(true);

            var model = new SupplierDeleteViewModel { SupplierId = supplierId.ToString() };

            var result = await _service.ConfirmDeleteAsync(model);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetDetailsAsync_ShouldReturnModel_WhenItemExists()
        {
            // Arrange
            var supplierId = Guid.NewGuid();
            var consumableName = "Consumable 1";
            var sparepartName = "Sparepart 1";

            var suppliers = new List<Supplier>
    {
        new Supplier
        {
            SupplierId = supplierId,
            Name = "Supplier 1",
            Address = "Address 1",
            Email = "email@example.com",
            PhoneNumber = "123456789",
            City = new City { Name = "City 1" },
            Country = new Country { Name = "Country 1" },
            Creator = new PMSUser { UserName = "test-user" },
            CreatedOn = DateTime.UtcNow,
            IsDleted = false
        }
    }.AsQueryable().BuildMock();

            var consumablesSuppliers = new List<ConsumableSupplier>
    {
        new ConsumableSupplier
        {
            SupplierId = supplierId,
            Consumable = new Consumable { Name = consumableName }
        }
    }.AsQueryable().BuildMock();

            var sparePartsSuppliers = new List<SparepartSupplier>
    {
        new SparepartSupplier
        {
            SupplierId = supplierId,
            Sparepart = new Sparepart { SparepartName = sparepartName }
        }
    }.AsQueryable().BuildMock();

            _suppliersRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(suppliers);
            _consumablesSuppliersRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(consumablesSuppliers);
            _sparePartsSuppliersRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(sparePartsSuppliers);

            // Act
            var result = await _service.GetDetailsAsync(supplierId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Supplier 1", result.Name);
            Assert.AreEqual("City 1", result.City);
            Assert.AreEqual("Country 1", result.Country);
            Assert.AreEqual("test-user", result.Creator);
            Assert.AreEqual(1, result.Consumables.Count);
            Assert.AreEqual(consumableName, result.Consumables.First());
            Assert.AreEqual(1, result.Spareparts.Count);
            Assert.AreEqual(sparepartName, result.Spareparts.First());
        }

    }
}

