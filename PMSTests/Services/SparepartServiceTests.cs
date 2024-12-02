using MockQueryable;
using Moq;
using PMS.Data.Models;
using PMS.Data.Models.Identity;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data;
using PMSWeb.ViewModels.SparepartVM;

namespace PMSTests.Services
{
    [TestFixture]
    public class SparepartServiceTests
    {
        private Mock<IRepository<Equipment, Guid>> _equipmentRepoMock;
        private Mock<IRepository<Sparepart, Guid>> _sparesRepoMock;
        private SparepartService _service;

        [SetUp]
        public void Setup()
        {
            _equipmentRepoMock = new Mock<IRepository<Equipment, Guid>>();
            _sparesRepoMock = new Mock<IRepository<Sparepart, Guid>>();
            _service = new SparepartService(_equipmentRepoMock.Object, _sparesRepoMock.Object);
        }

        [Test]
        public async Task GetListOfViewModelsAsync_ShouldReturnModels_WhenDataExists()
        {
            // Arrange
            var spareparts = new List<Sparepart>
    {
        new Sparepart
        {
            SparepartId = Guid.NewGuid(),
            SparepartName = "Spare 1",
            Units = "pcs",
            Description = "Test Sparepart 1",
            Price = 100.0M,
            ROB = 10,
            Equipment = new Equipment { Name = "Equipment 1" },
            IsDeleted = false,
            EditedOn = DateTime.UtcNow.AddMinutes(1) // Newer date
        },
        new Sparepart
        {
            SparepartId = Guid.NewGuid(),
            SparepartName = "Spare 2",
            Units = "kg",
            Description = "Test Sparepart 2",
            Price = 200.0M,
            ROB = 5,
            Equipment = new Equipment { Name = "Equipment 2" },
            IsDeleted = false,
            EditedOn = DateTime.UtcNow // Older date
        }
    }.AsQueryable().BuildMock(); // Convert to async queryable

            _sparesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(spareparts);

            // Act
            var result = await _service.GetListOfViewModelsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Spare 1", result.First().Name); // Newer item should come first
            Assert.AreEqual("Spare 2", result.Last().Name); // Older item should come last
        }


        [Test]
        public async Task GetListOfViewModelsAsync_ShouldReturnEmptyList_WhenNoData()
        {
            // Arrange
            var spareparts = new List<Sparepart>().AsQueryable(); // Empty list
            var mockSparepartsQueryable = spareparts.BuildMock(); // Converts to async queryable

            _sparesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(mockSparepartsQueryable);

            // Act
            var result = await _service.GetListOfViewModelsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result); // Should return an empty list
        }

        [Test]
        public async Task GetItemForCreateAsync_ShouldReturnModelWithEquipments_WhenDataExists()
        {
            // Arrange
            var equipmentList = new List<Equipment>
    {
        new Equipment { EquipmentId = Guid.NewGuid(), Name = "Equipment 1", IsDeleted = false },
        new Equipment { EquipmentId = Guid.NewGuid(), Name = "Equipment 2", IsDeleted = false }
    };
            var mockEquipmentQueryable = equipmentList.AsQueryable().BuildMock(); // Convert to async queryable

            _equipmentRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(mockEquipmentQueryable);

            // Act
            var result = await _service.GetItemForCreateAsync();

            // Assert
            Assert.IsNotNull(result.Equipments);
            Assert.AreEqual(2, result.Equipments.Count);
            Assert.AreEqual("Equipment 1", result.Equipments.First().Name);
        }

        [Test]
        public async Task GetItemForCreateAsync_ShouldReturnEmptyEquipments_WhenNoData()
        {
            // Arrange
            var equipments = new List<Equipment>().AsQueryable();
            var mockEquipmentQueryable = equipments.BuildMock(); // Convert to async queryable

            _equipmentRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(mockEquipmentQueryable);

            // Act
            var result = await _service.GetItemForCreateAsync();

            // Assert
            Assert.IsNotNull(result.Equipments);
            Assert.IsEmpty(result.Equipments);
        }

        [Test]
        public async Task CreateSparepartAsync_ShouldReturnTrue_WhenValidData()
        {
            // Arrange
            var model = new SparepartCreateViewModel
            {
                Name = "Spare 1",
                Description = "Test Description",
                ROB = 10,
                Price = 100,
                Units = "pcs",
                EquipmentId = Guid.NewGuid().ToString()
            };
            var userId = "test-user";

            _sparesRepoMock.Setup(repo => repo.AddAsync(It.IsAny<Sparepart>())).ReturnsAsync(true);

            // Act
            var result = await _service.CreateSparepartAsync(model, userId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task CreateSparepartAsync_ShouldReturnFalse_WhenExceptionOccurs()
        {
            // Arrange
            var model = new SparepartCreateViewModel
            {
                Name = "Spare 1",
                Description = "Test Description",
                ROB = 10,
                Price = 100,
                Units = "pcs",
                EquipmentId = Guid.NewGuid().ToString()
            };
            var userId = "test-user";

            _sparesRepoMock.Setup(repo => repo.AddAsync(It.IsAny<Sparepart>())).ThrowsAsync(new Exception());

            // Act
            var result = await _service.CreateSparepartAsync(model, userId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetItemForEditAsync_ShouldReturnEditModel_WhenSparePartExists()
        {
            // Arrange
            var sparepartId = Guid.NewGuid();
            var spareparts = new List<Sparepart>
    {
        new Sparepart
        {
            SparepartId = sparepartId,
            SparepartName = "Spare 1",
            Description = "Test Description",
            Price = 100,
            ROB = 10,
            Units = "pcs",
            ImageURL = "test-url",
            IsDeleted = false
        }
    }.AsQueryable().BuildMock(); // Converts to async queryable

            _sparesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(spareparts);

            // Act
            var result = await _service.GetItemForEditAsync(sparepartId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Spare 1", result.Name);
            Assert.AreEqual("Test Description", result.Description);
        }

        [Test]
        public async Task GetItemForEditAsync_ShouldReturnEmptyModel_WhenSparePartDoesNotExist()
        {
            // Arrange
            var spareparts = new List<Sparepart>().AsQueryable(); // Empty list to simulate no data
            var mockSparepartsQueryable = spareparts.BuildMock(); // Convert to async queryable

            _sparesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(mockSparepartsQueryable);

            // Act
            var result = await _service.GetItemForEditAsync(Guid.NewGuid().ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Name); // Model should be empty
            Assert.IsNull(result.Description);
        }

        [Test]
        public async Task SaveItemToEditAsync_ShouldReturnTrue_WhenUpdateSucceeds()
        {
            // Arrange
            var sparepartId = Guid.NewGuid();
            var spareparts = new List<Sparepart>
    {
        new Sparepart
        {
            SparepartId = sparepartId,
            SparepartName = "Old Sparepart Name",
            Description = "Old Description",
            Price = 100.0M,
            ROB = 10,
            Units = "pcs",
            ImageURL = "old-url",
            IsDeleted = false
        }
    }.AsQueryable().BuildMock(); // Convert to async queryable

            _sparesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(spareparts);
            _sparesRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Sparepart>())).ReturnsAsync(true);

            var editModel = new SparepartEditViewModel
            {
                SparepartId = sparepartId.ToString(),
                Name = "Updated Sparepart Name",
                Description = "Updated Description",
                Price = 150.0M,
                ROB = 20,
                Units = "pcs",
                ImageUrl = "updated-url"
            };

            // Act
            var result = await _service.SaveItemToEditAsync(editModel, "test-user");

            // Assert
            Assert.IsTrue(result);
        }


        [Test]
        public async Task SaveItemToEditAsync_ShouldReturnFalse_WhenSparePartNotFound()
        {
            // Arrange
            var spareparts = new List<Sparepart>().AsQueryable(); // Empty list to simulate no matching item
            var mockSparepartsQueryable = spareparts.BuildMock(); // Convert to async queryable

            _sparesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(mockSparepartsQueryable);

            var editModel = new SparepartEditViewModel
            {
                SparepartId = Guid.NewGuid().ToString(),
                Name = "New Sparepart Name",
                Description = "Updated Description",
                Price = 150.0M,
                ROB = 20,
                Units = "pcs"
            };

            // Act
            var result = await _service.SaveItemToEditAsync(editModel, "test-user");

            // Assert
            Assert.IsFalse(result);
        }

        // Add similar tests for the remaining methods: GetItemToDeleteAsync, ConfirmDeleteAsync, and GetDetailsAsync.
        [Test]
        public async Task GetItemToDeleteAsync_ShouldReturnModel_WhenItemExists()
        {
            // Arrange
            var sparepartId = Guid.NewGuid();
            var spareparts = new List<Sparepart>
    {
        new Sparepart
        {
            SparepartId = sparepartId,
            SparepartName = "Sparepart 1",
            Description = "Test Description",
            CreatedOn = DateTime.UtcNow,
            IsDeleted = false
        }
    }.AsQueryable().BuildMock(); // Convert to async queryable

            _sparesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(spareparts);

            // Act
            var result = await _service.GetItemToDeleteAsync(sparepartId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(sparepartId.ToString(), result.SparepartId);
            Assert.AreEqual("Sparepart 1", result.Name);
        }

        [Test]
        public async Task GetItemToDeleteAsync_ShouldReturnEmptyModel_WhenItemDoesNotExist()
        {
            // Arrange
            var spareparts = new List<Sparepart>().AsQueryable(); // Empty list
            _sparesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(spareparts.BuildMock());

            // Act
            var result = await _service.GetItemToDeleteAsync(Guid.NewGuid().ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Name); // Should return an empty model
            Assert.IsNull(result.Description);
        }

        [Test]
        public async Task ConfirmDeleteAsync_ShouldReturnTrue_WhenDeleteSucceeds()
        {
            // Arrange
            var sparepartId = Guid.NewGuid();
            var spareparts = new List<Sparepart>
    {
        new Sparepart
        {
            SparepartId = sparepartId,
            IsDeleted = false
        }
    }.AsQueryable().BuildMock(); // Convert to async queryable

            _sparesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(spareparts);
            _sparesRepoMock.Setup(repo => repo.RemoveItemAsync(It.IsAny<Sparepart>())).ReturnsAsync(true);

            var deleteModel = new SparepartDeleteViewModel { SparepartId = sparepartId.ToString() };

            // Act
            var result = await _service.ConfirmDeleteAsync(deleteModel);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ConfirmDeleteAsync_ShouldReturnFalse_WhenItemNotFound()
        {
            // Arrange
            var spareparts = new List<Sparepart>().AsQueryable(); // Empty list
            _sparesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(spareparts.BuildMock());

            var deleteModel = new SparepartDeleteViewModel { SparepartId = Guid.NewGuid().ToString() };

            // Act
            var result = await _service.ConfirmDeleteAsync(deleteModel);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetDetailsAsync_ShouldReturnModel_WhenItemExists()
        {
            // Arrange
            var sparepartId = Guid.NewGuid();
            var spareparts = new List<Sparepart>
    {
        new Sparepart
        {
            SparepartId = sparepartId,
            SparepartName = "Sparepart 1",
            Description = "Test Description",
            Price = 100.0M,
            ROB = 10,
            Units = "pcs",
            CreatedOn = DateTime.UtcNow,
            Equipment = new Equipment { Name = "Equipment 1" },
            Creator = new PMSUser { UserName = "TestUser" },
            ImageURL = "test-url",
            IsDeleted = false
        }
    }.AsQueryable().BuildMock(); // Convert to async queryable

            _sparesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(spareparts);

            // Act
            var result = await _service.GetDetailsAsync(sparepartId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Sparepart 1", result.Name);
            Assert.AreEqual("Test Description", result.Description);
            Assert.AreEqual("Equipment 1", result.Equipment);
            Assert.AreEqual("TestUser", result.CreatorName);
            Assert.AreEqual("test-url", result.ImageURL);
        }

        [Test]
        public async Task GetDetailsAsync_ShouldReturnEmptyModel_WhenItemDoesNotExist()
        {
            // Arrange
            var spareparts = new List<Sparepart>().AsQueryable(); // Empty list
            _sparesRepoMock.Setup(repo => repo.GetAllAsQueryable()).Returns(spareparts.BuildMock());

            // Act
            var result = await _service.GetDetailsAsync(Guid.NewGuid().ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Name); // Should return an empty model
            Assert.IsNull(result.Description);
        }



    }
}
