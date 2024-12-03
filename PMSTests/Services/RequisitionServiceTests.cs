using MockQueryable;
using MockQueryable.Moq;
using Moq;
using PMS.Data.Models;
using PMS.Data.Models.Identity;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data;
using PMSWeb.ViewModels.RequisitionVM;

namespace PMSTests.Services
{
    [TestFixture]
    public class RequisitionServiceTests
    {
        private Mock<IRepository<Requisition, Guid>> _mockRequisitionRepo;
        private Mock<IRepository<Sparepart, Guid>> _mockSparesRepo;
        private Mock<IRepository<Consumable, Guid>> _mockConsumablesRepo;
        private Mock<IRepository<RequisitionItem, Guid>> _mockRequisitionItemsRepo;
        private Mock<IRepository<Budget, Guid>> _mockBudgetRepo;
        private RequisitionService _requisitionService;

        [SetUp]
        public void Setup()
        {
            _mockRequisitionRepo = new Mock<IRepository<Requisition, Guid>>();
            _mockSparesRepo = new Mock<IRepository<Sparepart, Guid>>();
            _mockConsumablesRepo = new Mock<IRepository<Consumable, Guid>>();
            _mockRequisitionItemsRepo = new Mock<IRepository<RequisitionItem, Guid>>();
            _mockBudgetRepo = new Mock<IRepository<Budget, Guid>>();

            _requisitionService = new RequisitionService(
                _mockRequisitionRepo.Object,
                _mockSparesRepo.Object,
                _mockConsumablesRepo.Object,
                _mockRequisitionItemsRepo.Object,
                _mockBudgetRepo.Object
            );
        }

        [Test]
        public async Task GetAllItemsListAsync_ReturnsCorrectList()
        {
            // Arrange
            var requisitions = new List<Requisition>
            {
                new Requisition
                {
                    RequisitionId = Guid.NewGuid(),
                    RequisitionName = "Requisition A",
                    CreatedOn = DateTime.Now,
                    IsApproved = false,
                    RequisitionType = "spareparts",
                    Creator = new PMSUser { UserName = "UserA" },
                    TotalCost = 500
                },
                new Requisition
                {
                    RequisitionId = Guid.NewGuid(),
                    RequisitionName = "Requisition B",
                    CreatedOn = DateTime.Now.AddDays(-1),
                    IsApproved = true,
                    RequisitionType = "consumables",
                    Creator = new PMSUser { UserName = "UserB" },
                    TotalCost = 300
                }
            };

            var mockDbSet = requisitions.AsQueryable().BuildMockDbSet();
            _mockRequisitionRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _requisitionService.GetAllItemsListAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Requisition A", result.First().RequisitionName);
        }

        [Test]
        public async Task CreateRequisitionAsync_WithValidData_ReturnsTrue()
        {
            // Arrange
            var createModel = new RequisitionCreateViewModel
            {
                RequisitionName = "New Requisition",
                RequisitionType = "spareparts",
                RequisitionItems = new List<RequisitionItemViewModel>
                {
                    new RequisitionItemViewModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Sparepart A",
                        ToOrdered = 10,
                        Price = 50,
                        IsSelected = true
                    }
                }
            };

            _mockRequisitionRepo.Setup(r => r.AddAsync(It.IsAny<Requisition>())).ReturnsAsync(true);

            // Act
            var result = await _requisitionService.CreateRequisitionAsync(createModel, "user123");

            // Assert
            Assert.IsTrue(result);
            _mockRequisitionRepo.Verify(r => r.AddAsync(It.Is<Requisition>(req =>
                req.RequisitionName == "New Requisition" &&
                req.RequisitionType == "spareparts" &&
                req.TotalCost == 500)), Times.Once);
        }


        [Test]
        public async Task DeleteRequisitionAsync_WithValidId_ReturnsTrue()
        {
            // Arrange
            var requisitionId = Guid.NewGuid();
            var requisition = new Requisition
            {
                RequisitionId = requisitionId,
                IsDeleted = false
            };

            _mockRequisitionRepo.Setup(r => r.GetAllAsQueryable()).Returns(new List<Requisition> { requisition }.AsQueryable().BuildMockDbSet().Object);
            _mockRequisitionRepo.Setup(r => r.UpdateAsync(It.IsAny<Requisition>())).ReturnsAsync(true);

            // Act
            var result = await _requisitionService.DeleteRequisitionAsync(requisitionId.ToString());

            // Assert
            Assert.IsTrue(result);
            _mockRequisitionRepo.Verify(r => r.UpdateAsync(It.Is<Requisition>(req => req.IsDeleted)), Times.Once);
        }

        [Test]
        public async Task GetAllReadyForApprovalAsync_ReturnsCorrectList()
        {
            // Arrange
            var requisitions = new List<Requisition>
    {
        new Requisition
        {
            RequisitionId = Guid.NewGuid(),
            RequisitionName = "Requisition A",
            CreatedOn = DateTime.UtcNow,
            IsApproved = false,
            IsDeleted = false,
            RequisitionType = "spareparts",
            Creator = new PMSUser { UserName = "UserA" },
            TotalCost = 200
        }
    };

            var mockDbSet = requisitions.AsQueryable().BuildMockDbSet();
            _mockRequisitionRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _requisitionService.GetAllReadyForApprovalAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Requisition A", result.First().RequisitionName);
        }

        [Test]
        public async Task GetAllApprovedAsync_ReturnsCorrectList()
        {
            // Arrange
            var requisitions = new List<Requisition>
    {
        new Requisition
        {
            RequisitionId = Guid.NewGuid(),
            RequisitionName = "Approved Requisition",
            CreatedOn = DateTime.UtcNow,
            IsApproved = true,
            IsDeleted = false,
            RequisitionType = "consumables",
            Creator = new PMSUser { UserName = "UserB" },
            TotalCost = 300
        }
    };

            var mockDbSet = requisitions.AsQueryable().BuildMockDbSet();
            _mockRequisitionRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _requisitionService.GetAllApprovedAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Approved Requisition", result.First().RequisitionName);
        }

        [Test]
        public async Task GetRequisitionDetailsModelAsync_ReturnsCorrectDetails()
        {
            // Arrange
            var requisitionId = Guid.NewGuid();
            var requisition = new Requisition
            {
                RequisitionId = requisitionId,
                RequisitionName = "Detailed Requisition",
                CreatedOn = DateTime.UtcNow,
                IsApproved = true,
                IsDeleted = false,
                TotalCost = 100,
                RequisitionType = "spareparts",
                Creator = new PMSUser { UserName = "UserC" }
            };

            var requisitionItems = new List<RequisitionItem>
    {
        new RequisitionItem
        {
            ItemId = Guid.NewGuid(),
            Name = "Item A",
            OrderedAmount = 5,
            Price = 20,
            SupplierName = "SupplierA",
            RequisitionId = requisitionId
        }
    };

            _mockRequisitionRepo.Setup(r => r.GetAllAsQueryable()).Returns(new List<Requisition> { requisition }.AsQueryable().BuildMockDbSet().Object);
            _mockRequisitionItemsRepo.Setup(r => r.GetAllAsQueryable()).Returns(requisitionItems.AsQueryable().BuildMockDbSet().Object);

            // Act
            var result = await _requisitionService.GetRequisitionDetailsModelAsync(requisitionId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Detailed Requisition", result.RequisitionName);
            Assert.AreEqual(1, result.requisitionItems.Count);
            Assert.AreEqual("Item A", result.requisitionItems.First().Name);
        }

        [Test]
        public async Task ApproveRequisition_WithValidId_ReturnsCorrectMessage()
        {
            // Arrange
            var requisitionId = Guid.NewGuid();
            var requisition = new Requisition
            {
                RequisitionId = requisitionId,
                IsApproved = false,
                IsDeleted = false,
                TotalCost = 100,
                RequisitionType = "consumables",
                requisitionItems = new List<RequisitionItem>
        {
            new RequisitionItem
            {
                PurchasedItemId = Guid.NewGuid(),
                OrderedAmount = 10
            }
        }
            };

            var budget = new Budget { Ballance = 200, LastChangeDate = DateTime.UtcNow };

            var consumables = new List<Consumable>
    {
        new Consumable { ConsumableId = Guid.NewGuid(), ROB = 5, EditedOn = DateTime.UtcNow }
    };

            _mockRequisitionRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<Requisition> { requisition }.AsQueryable().BuildMockDbSet().Object);

            _mockBudgetRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<Budget> { budget }.AsQueryable().BuildMockDbSet().Object);

            _mockConsumablesRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(consumables.AsQueryable().BuildMockDbSet().Object);

            _mockRequisitionRepo.Setup(r => r.UpdateAsync(It.IsAny<Requisition>())).ReturnsAsync(true);
            _mockBudgetRepo.Setup(r => r.UpdateAsync(It.IsAny<Budget>())).ReturnsAsync(true);
            _mockConsumablesRepo.Setup(r => r.UpdateRange(It.IsAny<IEnumerable<Consumable>>())).ReturnsAsync(true);

            // Act
            var result = await _requisitionService.ApproveRequisition(requisitionId.ToString());

            // Assert
            Assert.AreEqual("Consumables", result);
            _mockRequisitionRepo.Verify(r => r.UpdateAsync(It.Is<Requisition>(req => req.IsApproved)), Times.Once);
            _mockBudgetRepo.Verify(r => r.UpdateAsync(It.Is<Budget>(b => b.Ballance == 100)), Times.Once);
            _mockConsumablesRepo.Verify(r => r.UpdateRange(It.IsAny<IEnumerable<Consumable>>()), Times.Once);
        }


        [Test]
        public async Task DeleteRequisitionAsync_RepositoryThrowsException_ReturnsFalse()
        {
            // Arrange
            var requisitionId = Guid.NewGuid();
            _mockRequisitionRepo.Setup(r => r.GetAllAsQueryable()).Throws(new Exception("Database error"));

            // Act
            var result = await _requisitionService.DeleteRequisitionAsync(requisitionId.ToString());

            // Assert
            Assert.IsFalse(result);
        }


        [Test]
        public async Task GetCreateSparesRequisitionModelAsync_ReturnsValidModel()
        {
            // Arrange
            var spareId = Guid.NewGuid();

            var spares = new List<Sparepart>
    {
        new Sparepart
        {
            SparepartId = spareId,
            SparepartName = "Spare A",
            ROB = 10,
            Units = "pcs",
            Price = 50,
            SparepartsSuppliers = new List<SparepartSupplier>
            {
                new SparepartSupplier { Supplier = new Supplier { Name = "Supplier A" } }
            }
        }
    };

            _mockSparesRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(spares.AsQueryable().BuildMockDbSet().Object);

            // Act
            var result = await _requisitionService.GetCreateSparesRequisitionModelAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("spareparts", result.RequisitionType);
            Assert.AreEqual(1, result.RequisitionItems.Count);
            Assert.AreEqual("Spare A", result.RequisitionItems.First().Name);
        }

        [Test]
        public async Task ApproveRequisition_WithLowBudget_ReturnsLowBallance()
        {
            // Arrange
            var requisitionId = Guid.NewGuid();
            var requisition = new Requisition
            {
                RequisitionId = requisitionId,
                IsApproved = false,
                IsDeleted = false,
                TotalCost = 300
            };

            var budget = new Budget { Ballance = 100, LastChangeDate = DateTime.UtcNow };

            _mockRequisitionRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<Requisition> { requisition }.AsQueryable().BuildMockDbSet().Object);

            _mockBudgetRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<Budget> { budget }.AsQueryable().BuildMockDbSet().Object);

            // Act
            var result = await _requisitionService.ApproveRequisition(requisitionId.ToString());

            // Assert
            Assert.AreEqual("LowBallance", result);
        }

        [Test]
        public async Task ApproveRequisition_WithInvalidId_ReturnsNullOrApproved()
        {
            // Arrange
            var requisitionId = Guid.NewGuid();

            _mockRequisitionRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<Requisition>().AsQueryable().BuildMockDbSet().Object);

            // Act
            var result = await _requisitionService.ApproveRequisition(requisitionId.ToString());

            // Assert
            Assert.AreEqual("NullOrApproved", result);
        }

        [Test]
        public async Task GetRequisitionDeleteViewModelAsync_ReturnsValidModel()
        {
            // Arrange
            var requisitionId = Guid.NewGuid();
            var requisition = new Requisition
            {
                RequisitionId = requisitionId,
                RequisitionName = "Requisition To Delete",
                CreatedOn = DateTime.UtcNow,
                RequisitionType = "spareparts",
                IsDeleted = false
            };

            _mockRequisitionRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<Requisition> { requisition }.AsQueryable().BuildMockDbSet().Object);

            // Act
            var result = await _requisitionService.GetRequisitionDeleteViewModelAsync(requisitionId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Requisition To Delete", result.RequisitionName);
            Assert.AreEqual("spareparts", result.RequisitionType);
        }

        [Test]
        public async Task GetAllItemsListAsync_WhenNoItems_ReturnsEmptyList()
        {
            // Arrange
            _mockRequisitionRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<Requisition>().AsQueryable().BuildMockDbSet().Object);

            // Act
            var result = await _requisitionService.GetAllItemsListAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }


    }
}
