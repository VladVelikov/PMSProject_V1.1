using MockQueryable.Moq;
using Moq;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data;
using PMSWeb.ViewModels.JobOrderVM;

namespace PMSTests.Services
{
    [TestFixture]
    public class JobOrderServiceTests
    {
        private Mock<IRepository<JobOrder, Guid>> _mockJobOrdersRepo;
        private Mock<IRepository<Equipment, Guid>> _mockEquipmentRepo;
        private Mock<IRepository<RoutineMaintenance, Guid>> _mockRoutineMaintRepo;
        private Mock<IRepository<SpecificMaintenance, Guid>> _mockSpecMaintRepo;
        private JoborderService _jobOrderService;

        [SetUp]
        public void Setup()
        {
            _mockJobOrdersRepo = new Mock<IRepository<JobOrder, Guid>>();
            _mockEquipmentRepo = new Mock<IRepository<Equipment, Guid>>();
            _mockRoutineMaintRepo = new Mock<IRepository<RoutineMaintenance, Guid>>();
            _mockSpecMaintRepo = new Mock<IRepository<SpecificMaintenance, Guid>>();

            _jobOrderService = new JoborderService(
                _mockJobOrdersRepo.Object,
                _mockRoutineMaintRepo.Object,
                _mockSpecMaintRepo.Object,
                _mockEquipmentRepo.Object,
                null,
                null,
                null,
                null,
                null
            );
        }

        [Test]
        public async Task GetListOfAllJobsAsync_ReturnsCorrectList()
        {
            // Arrange
            var jobOrders = new List<JobOrder>
            {
                new JobOrder
                {
                    JobId = Guid.NewGuid(),
                    JobName = "Job A",
                    DueDate = DateTime.Now.AddDays(1),
                    LastDoneDate = DateTime.Now.AddDays(-5),
                    Equipment = new Equipment { Name = "Equipment A", IsDeleted = false },
                    IsDeleted = false,
                    IsHistory = false,
                    Type = "Routine",
                    ResponsiblePosition = "Technician"
                },
                new JobOrder
                {
                    JobId = Guid.NewGuid(),
                    JobName = "Job B",
                    DueDate = DateTime.Now.AddDays(2),
                    LastDoneDate = DateTime.Now.AddDays(-10),
                    Equipment = new Equipment { Name = "Equipment B", IsDeleted = false },
                    IsDeleted = false,
                    IsHistory = false,
                    Type = "Specific",
                    ResponsiblePosition = "Engineer"
                }
            };

            var mockDbSet = jobOrders.AsQueryable().BuildMockDbSet();
            _mockJobOrdersRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _jobOrderService.GetListOfAllJobsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Job A", result[0].JobName);
            Assert.AreEqual("Equipment A", result[0].EquipmentName);
        }

        [Test]
        public async Task GetListOfDueJobsAsync_ReturnsDueJobs()
        {
            // Arrange
            var jobOrders = new List<JobOrder>
            {
                new JobOrder
                {
                    JobId = Guid.NewGuid(),
                    JobName = "Due Job A",
                    DueDate = DateTime.UtcNow.AddDays(-1),
                    LastDoneDate = DateTime.UtcNow.AddDays(-10),
                    Equipment = new Equipment { Name = "Equipment A", IsDeleted = false },
                    IsDeleted = false,
                    IsHistory = false
                },
                new JobOrder
                {
                    JobId = Guid.NewGuid(),
                    JobName = "Not Due Job",
                    DueDate = DateTime.UtcNow.AddDays(5),
                    LastDoneDate = DateTime.UtcNow.AddDays(-5),
                    Equipment = new Equipment { Name = "Equipment B", IsDeleted = false },
                    IsDeleted = false,
                    IsHistory = false
                }
            };

            var mockDbSet = jobOrders.AsQueryable().BuildMockDbSet();
            _mockJobOrdersRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _jobOrderService.GetListOfDueJobsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Due Job A", result[0].JobName);
        }

        [Test]
        public async Task GetHistoryDetailsAsync_WithValidId_ReturnsDetails()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var jobOrder = new JobOrder
            {
                JobId = jobId,
                JobName = "Historical Job",
                LastDoneDate = DateTime.UtcNow.AddDays(-10),
                CompletedBy = "John Doe",
                JobDescription = "Routine maintenance job",
                Equipment = new Equipment { Name = "Equipment A" },
                IsDeleted = false,
                IsHistory = true
            };

            var mockDbSet = new List<JobOrder> { jobOrder }.AsQueryable().BuildMockDbSet();
            _mockJobOrdersRepo.Setup(r => r.GetAllAsQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _jobOrderService.GetHistoryDetailsAsync(jobId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Historical Job", result.JobName);
            Assert.AreEqual("Equipment A", result.MaintainedEquipment);
            Assert.AreEqual("John Doe", result.CompletedBy);
        }

        [Test]
        public async Task CreateJobOrderAsync_WithValidData_ReturnsTrue()
        {
            // Arrange
            var createModel = new JobOrderCreateViewModel
            {
                JobName = "New Job",
                JobDescription = "Routine maintenance",
                DueDate = DateTime.UtcNow.AddDays(10),
                LastDoneDate = DateTime.UtcNow,
                Interval = 30,
                Type = "Routine",
                ResponsiblePosition = "Technician",
                EquipmentId = Guid.NewGuid()
            };

            _mockJobOrdersRepo.Setup(r => r.AddAsync(It.IsAny<JobOrder>())).ReturnsAsync(true);

            // Act
            var result = await _jobOrderService.CreateJobOrderAsync(createModel, "user123");

            // Assert
            Assert.IsTrue(result);
            _mockJobOrdersRepo.Verify(r => r.AddAsync(It.Is<JobOrder>(job =>
                job.JobName == "New Job" &&
                job.CreatorId == "user123" &&
                job.DueDate == createModel.DueDate)), Times.Once);
        }

        [Test]
        public async Task DeleteJobOrderAsync_WithValidId_ReturnsTrue()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var jobOrder = new JobOrder { JobId = jobId, IsDeleted = false };
            _mockJobOrdersRepo.Setup(r => r.GetByIdAsync(jobId)).ReturnsAsync(jobOrder);
            _mockJobOrdersRepo.Setup(r => r.UpdateAsync(It.IsAny<JobOrder>())).ReturnsAsync(true);

            // Act
            var result = await _jobOrderService.DeleteJobOrderAsync(jobId.ToString());

            // Assert
            Assert.IsTrue(result);
            _mockJobOrdersRepo.Verify(r => r.UpdateAsync(It.Is<JobOrder>(job => job.IsDeleted)), Times.Once);
        }
    }
}
