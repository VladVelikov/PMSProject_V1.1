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

    }
}
