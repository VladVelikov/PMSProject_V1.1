using MockQueryable.Moq;
using Moq;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data;
using PMSWeb.ViewModels.InventoryVM;
using PMSWeb.ViewModels.JobOrderVM;

namespace PMSTests.Services
{
    [TestFixture]
    public class JobOrderServiceTests2
    {
        private Mock<IRepository<JobOrder, Guid>> _mockJobOrdersRepo;
        private Mock<IRepository<Equipment, Guid>> _mockEquipmentRepo;
        private Mock<IRepository<RoutineMaintenance, Guid>> _mockRoutineMaintRepo;
        private Mock<IRepository<SpecificMaintenance, Guid>> _mockSpecMaintRepo;
        private Mock<IRepository<Sparepart, Guid>> _mockSparesRepo;
        private Mock<IRepository<Consumable, Guid>> _mockConsumableRepo;
        private Mock<IRepository<Manual, Guid>> _mockManualsRepo;
        private JoborderService _jobOrderService;

        [SetUp]
        public void Setup()
        {
            _mockJobOrdersRepo = new Mock<IRepository<JobOrder, Guid>>();
            _mockEquipmentRepo = new Mock<IRepository<Equipment, Guid>>();
            _mockRoutineMaintRepo = new Mock<IRepository<RoutineMaintenance, Guid>>();
            _mockSpecMaintRepo = new Mock<IRepository<SpecificMaintenance, Guid>>();
            _mockSparesRepo = new Mock<IRepository<Sparepart, Guid>>();
            _mockConsumableRepo = new Mock<IRepository<Consumable, Guid>>();
            _mockManualsRepo = new Mock<IRepository<Manual, Guid>>();

            _jobOrderService = new JoborderService(
                _mockJobOrdersRepo.Object,
                _mockRoutineMaintRepo.Object,
                _mockSpecMaintRepo.Object,
                _mockEquipmentRepo.Object,
                null,
                _mockSparesRepo.Object,
                _mockConsumableRepo.Object,
                null,
                _mockManualsRepo.Object
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

        [Test]
        public async Task GetListOfAllJobsAsync_WhenNoJobs_ReturnsEmptyList()
        {
            // Arrange
            _mockJobOrdersRepo.Setup(r => r.GetAllAsQueryable()).Returns(new List<JobOrder>().AsQueryable().BuildMockDbSet().Object);

            // Act
            var result = await _jobOrderService.GetListOfAllJobsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public async Task GetHistoryDetailsAsync_WithInvalidId_ReturnsEmptyModel()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _mockJobOrdersRepo.Setup(r => r.GetAllAsQueryable()).Returns(new List<JobOrder>().AsQueryable().BuildMockDbSet().Object);

            // Act
            var result = await _jobOrderService.GetHistoryDetailsAsync(invalidId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.JobName);
        }

        [Test]
        public async Task GetAddSpecificMaintenanceViewModelAsync_ReturnsValidModel()
        {
            // Arrange
            var equipmentId = Guid.NewGuid();
            var specificMaintenanceId = Guid.NewGuid();

            var equipment = new Equipment { EquipmentId = equipmentId, Name = "Equipment A", IsDeleted = false };
            var specificMaintenances = new List<SpecificMaintenance>
    {
        new SpecificMaintenance { SpecMaintId = specificMaintenanceId, Name = "Specific Maintenance A", EquipmentId = equipmentId }
    };

            _mockEquipmentRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<Equipment> { equipment }.AsQueryable().BuildMockDbSet().Object);

            _mockSpecMaintRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(specificMaintenances.AsQueryable().BuildMockDbSet().Object);

            // Act
            var result = await _jobOrderService.GetAddSpecificMaintenanceViewModelAsync(equipmentId, "Specific");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Equipment A", result.EquipmentName);
            Assert.AreEqual(1, result.Maintenances.Count);
            Assert.AreEqual("Specific Maintenance A", result.Maintenances[0].Name);
        }

        [Test]
        public async Task ConfirmSparesAreUsedAsync_UpdatesSpareQuantities()
        {
            // Arrange
            var spareId = Guid.NewGuid();
            var equipmentId = Guid.NewGuid();

            var spares = new List<Sparepart>
    {
        new Sparepart { SparepartId = spareId, ROB = 10, Units = "pcs", EquipmentId = equipmentId }
    };

            _mockSparesRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(spares.AsQueryable().BuildMockDbSet().Object);

            var model = new PartialViewModel
            {
                EquipmentId = equipmentId.ToString(),
                InventoryList = new List<InventoryItemViewModel>
        {
            new InventoryItemViewModel { Id = spareId.ToString(), Used = 5 }
        }
            };

            // Act
            var result = await _jobOrderService.ConfirmSparesAreUsedAsync(model);

            // Assert
            Assert.IsTrue(result);
            _mockSparesRepo.Verify(r => r.UpdateAsync(It.Is<Sparepart>(s => s.ROB == 5)), Times.Once);
        }

        [Test]
        public async Task GetCompleteJobModelAsync_ReturnsJobDetails()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var jobOrder = new JobOrder
            {
                JobId = jobId,
                JobName = "Complete Job",
                JobDescription = "Job Description",
                DueDate = DateTime.UtcNow.AddDays(5),
                Equipment = new Equipment { Name = "Equipment A" },
                IsDeleted = false,
                IsHistory = false
            };

            _mockJobOrdersRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<JobOrder> { jobOrder }.AsQueryable().BuildMockDbSet().Object);

            // Act
            var result = await _jobOrderService.GetCompleteJobModelAsync(jobId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Complete Job", result.JobName);
            Assert.AreEqual("Equipment A", result.Equipment);
            Assert.AreEqual("Job Description", result.Description);
        }

        [Test]
        public async Task GetOpenManualViewModelAsync_ReturnsManualDetails()
        {
            // Arrange
            var manualId = Guid.NewGuid();
            var jobId = Guid.NewGuid();

            var manuals = new List<Manual>
        {
            new Manual
            {
                ManualId = manualId,
                ManualName = "Manual A",
                ContentURL = "http://example.com/manual.pdf",
                Maker = new Maker { MakerName = "Maker A" },
                Equipment = new Equipment { Name = "Equipment A" },
                IsDeleted = false
            }
        };

            _mockManualsRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(manuals.AsQueryable().BuildMockDbSet().Object);

            // Act
            var result = await _jobOrderService.GetOpenManualViewModelAsync(jobId.ToString(), manualId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Manual A", result.Name);
            Assert.AreEqual("http://example.com/manual.pdf", result.URL);
            Assert.AreEqual("Maker A", result.MakerName);
            Assert.AreEqual("Equipment A", result.EquipmentName);
        }

        [Test]
        public async Task DeleteJobOrderAsync_WithInvalidId_ReturnsFalse()
        {
            // Arrange
            _mockJobOrdersRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((JobOrder)null);

            // Act
            var result = await _jobOrderService.DeleteJobOrderAsync(Guid.NewGuid().ToString());

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteJobOrderAsync_RepositoryThrowsException_ReturnsFalse()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            _mockJobOrdersRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _jobOrderService.DeleteJobOrderAsync(jobId.ToString());

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task CloseThisJob_ValidJob_ClosesJobAndCreatesNewJob()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var equipmentId = Guid.NewGuid();
            var jobOrder = new JobOrder
            {
                JobId = jobId,
                JobName = "Old Job",
                LastDoneDate = DateTime.UtcNow.AddDays(-10),
                Interval = 30,
                IsHistory = false,
                EquipmentId = equipmentId,
                IsDeleted = false
            };

            _mockJobOrdersRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<JobOrder> { jobOrder }.AsQueryable().BuildMockDbSet().Object);

            _mockJobOrdersRepo.Setup(r => r.UpdateAsync(It.IsAny<JobOrder>())).ReturnsAsync(true);
            _mockJobOrdersRepo.Setup(r => r.AddAsync(It.IsAny<JobOrder>())).ReturnsAsync(true);

            var completeJobModel = new CompleteTheJobViewModel
            {
                JobId = jobId.ToString(),
                Details = "Job completed successfully"
            };

            // Act
            var result = await _jobOrderService.CloseThisJob(completeJobModel, "John Doe");

            // Assert
            Assert.IsTrue(result);
            _mockJobOrdersRepo.Verify(r => r.UpdateAsync(It.Is<JobOrder>(job => job.IsHistory && job.CompletedBy == "John Doe")), Times.Once);
            _mockJobOrdersRepo.Verify(r => r.AddAsync(It.Is<JobOrder>(job => job.DueDate == jobOrder.LastDoneDate.AddDays(jobOrder.Interval))), Times.Once);
        }

        [Test]
        public async Task CloseThisJob_InvalidJob_ReturnsFalse()
        {
            // Arrange
            _mockJobOrdersRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<JobOrder>().AsQueryable().BuildMockDbSet().Object);

            var completeJobModel = new CompleteTheJobViewModel { JobId = Guid.NewGuid().ToString() };

            // Act
            var result = await _jobOrderService.CloseThisJob(completeJobModel, "John Doe");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetAddEquipmentModelAsync_ReturnsEquipmentList()
        {
            // Arrange
            var equipmentList = new List<Equipment>
    {
        new Equipment { EquipmentId = Guid.NewGuid(), Name = "Equipment A", IsDeleted = false },
        new Equipment { EquipmentId = Guid.NewGuid(), Name = "Equipment B", IsDeleted = false }
    };

            _mockEquipmentRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(equipmentList.AsQueryable().BuildMockDbSet().Object);

            // Act
            var result = await _jobOrderService.GetAddEquipmentModelAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.EquipmentList.Count);
            Assert.AreEqual("Equipment A", result.EquipmentList[0].Name);
            Assert.AreEqual("Equipment B", result.EquipmentList[1].Name);
        }

        [Test]
        public async Task GetListOfDueJobsAsync_WhenNoDueJobs_ReturnsEmptyList()
        {
            // Arrange
            _mockJobOrdersRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<JobOrder>().AsQueryable().BuildMockDbSet().Object);

            // Act
            var result = await _jobOrderService.GetListOfDueJobsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public async Task GetCompleteJobModelAsync_InvalidId_ReturnsEmptyModel()
        {
            // Arrange
            var invalidId = Guid.NewGuid().ToString();
            _mockJobOrdersRepo.Setup(r => r.GetAllAsQueryable())
                .Returns(new List<JobOrder>().AsQueryable().BuildMockDbSet().Object);

            // Act
            var result = await _jobOrderService.GetCompleteJobModelAsync(invalidId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.JobName);
        }

        [Test]
        public async Task GetHistoryDetailsAsync_RepositoryThrowsException_ReturnsEmptyModel()
        {
            // Arrange
            _mockJobOrdersRepo.Setup(r => r.GetAllAsQueryable()).Throws(new Exception("Database error"));

            // Act
            var result = await _jobOrderService.GetHistoryDetailsAsync(Guid.NewGuid().ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.JobName);
        }

        [TestCase("invalid-id1", false)]
        [TestCase("invalid-id2", false)]
        public async Task DeleteJobOrderAsync_VariedInputs_ReturnsExpectedResult(string id, bool expectedResult)
        {
            // Arrange
            if (expectedResult)
            {
                var jobOrder = new JobOrder { JobId = Guid.Parse(id), IsDeleted = false };
                _mockJobOrdersRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(jobOrder);
                _mockJobOrdersRepo.Setup(r => r.UpdateAsync(It.IsAny<JobOrder>())).ReturnsAsync(true);
            }
            else
            {
                _mockJobOrdersRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((JobOrder)null);
            }

            // Act
            var result = await _jobOrderService.DeleteJobOrderAsync(id);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }





    }
}
