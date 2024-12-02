using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMS.Services.Data.Interfaces;
using PMSWeb.Controllers;
using PMSWeb.ViewModels.JobOrderVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PMSTests.Controllers
{
    [TestFixture]
    public class JobOrderControllerTests
    {
        private Mock<IJoborderService> _joborderServiceMock;
        private JobOrderController _controller;

        [SetUp]
        public void SetUp()
        {
            _joborderServiceMock = new Mock<IJoborderService>();
            _controller = new JobOrderController(_joborderServiceMock.Object);

            // Mock user identity
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        }));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test]
        public async Task Select_ReturnsRedirectToEmptyList_WhenNoJobsExist()
        {
            // Arrange
            _joborderServiceMock.Setup(service => service.GetListOfAllJobsAsync())
                .ReturnsAsync((List<JobOrderDisplayViewModel>)null); // Return null to trigger the redirect

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("EmptyList", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Select_ReturnsViewWithJobs_WhenJobsExist()
        {
            // Arrange
            var jobs = new List<JobOrderDisplayViewModel>
        {
            new JobOrderDisplayViewModel { JobId = Guid.NewGuid().ToString(), JobName = "Test Job 1" },
            new JobOrderDisplayViewModel { JobId = Guid.NewGuid().ToString(), JobName = "Test Job 2" }
        };
            _joborderServiceMock.Setup(service => service.GetListOfAllJobsAsync()).ReturnsAsync(jobs);

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(jobs, viewResult.Model);
        }

        [Test]
        public async Task Create_Get_ReturnsRedirectToWrongData_WhenInputModelIsInvalid()
        {
            // Arrange
            var inputModel = new JobOrderAddMaintenanceViewModel { TypeId = "InvalidType" };

            // Act
            var result = await _controller.Create(inputModel);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("WrongData", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Create_Get_ReturnsRedirectToWrongData_WhenServiceReturnsInvalidModel()
        {
            // Arrange
            var inputModel = new JobOrderAddMaintenanceViewModel
            {
                TypeId = "Routine",
                EquipmentId = Guid.NewGuid(),
                MaintenanceId = Guid.NewGuid(),
                EquipmentName = "Test Equipment"
            };
            _joborderServiceMock.Setup(service => service.GetCreateJobModelAsync(inputModel))
                .ReturnsAsync((JobOrderCreateViewModel)null);

            // Act
            var result = await _controller.Create(inputModel);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("WrongData", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Create_Post_RedirectsToSelect_WhenSuccessful()
        {
            // Arrange
            var model = new JobOrderCreateViewModel
            {
                JobName = "Test Job",
                DueDate = DateTime.UtcNow,
                LastDoneDate = DateTime.UtcNow,
                Interval = 10,
                Type = "Routine",
                ResponsiblePosition = "Manager",
                EquipmentId = Guid.NewGuid(),
                SpecificMaintenanceId = Guid.NewGuid()
            };
            _joborderServiceMock.Setup(service => service.CreateJobOrderAsync(model, It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(JobOrderController.Select), redirectResult.ActionName);
        }

        [Test]
        public async Task CompleteJob_ReturnsRedirectToWrongData_WhenIdIsInvalid()
        {
            // Act
            var result = await _controller.CompleteJob("invalid-guid");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("WrongData", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task CompleteJob_ReturnsRedirectToNotFound_WhenServiceReturnsNull()
        {
            // Arrange
            _joborderServiceMock.Setup(service => service.GetCompleteJobModelAsync(It.IsAny<string>()))
                .ReturnsAsync((CompleteTheJobViewModel)null);

            // Act
            var result = await _controller.CompleteJob(Guid.NewGuid().ToString());

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("NotFound", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task CompleteJob_ReturnsViewWithModel_WhenSuccessful()
        {
            // Arrange
            var model = new CompleteTheJobViewModel { JobId = Guid.NewGuid().ToString(), Details = "Details" };
            _joborderServiceMock.Setup(service => service.GetCompleteJobModelAsync(It.IsAny<string>())).ReturnsAsync(model);

            // Act
            var result = await _controller.CompleteJob(model.JobId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }
    }

}
